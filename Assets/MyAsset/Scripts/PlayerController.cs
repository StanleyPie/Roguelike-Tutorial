using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_BoardManager;
    private Vector2Int m_CellPosition;
    private bool m_isGameOver = false;

    public float speed = 5f;
    private bool m_isMoving;
    private Vector3 m_moveTarget;

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void GameOver()
    {
        this.m_isGameOver = true;
    }

    public void Init()
    {
        this.m_isMoving = false;
        this.m_isGameOver = false ;
    }

    public virtual void Spawn(BoardManager newBoardManager, Vector2Int newPos)
    {
        m_BoardManager = newBoardManager;
        this.MoveTo(newPos, true);
    }

    public virtual void MoveTo(Vector2Int newPos, bool immediate = false)
    {
        this.m_CellPosition = newPos;

        if (!immediate)
        {
            m_isMoving = true;
            m_moveTarget = m_BoardManager.CellToWorld(newPos);

        }
        else
        {
            m_isMoving = false;
            transform.position = m_BoardManager.CellToWorld(newPos);
        }
    }

    public void Update()
    {
        if (this.m_isGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.instance.StartNewGame();
            }
            return;

        }

        if (this.m_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_moveTarget, speed * Time.deltaTime);

            if (transform.position == m_moveTarget)
            {
                m_isMoving = false;
                m_Animator.SetBool ("moving" , false);

                var cellData = m_BoardManager.GetCellData(m_CellPosition);
                if (cellData.containGameObj != null)
                {
                    cellData.containGameObj.PlayerEnter();
                }
            }
            return;
        }

        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }
        if (hasMoved)
        {
            BoardManager.CellData cellData = m_BoardManager.GetCellData(newCellTarget);

            if (cellData != null && cellData.passable)
            {
                GameManager.instance.m_turnPlayer.Tick();

                if (cellData.containGameObj == null)
                {
                    m_Animator.SetBool("moving", true);
                    this.MoveTo(newCellTarget);
                }
                else
                {
                    StartCoroutine(this.AttackDura());

                    if (cellData.containGameObj.PlayerWantsToEnter())
                    {
                        MoveTo(newCellTarget);
                    }
                }
            }
        }
    }

    IEnumerator AttackDura()
    {
        m_Animator.SetBool("attack", true);

        yield return new WaitForSeconds(0.1f);

        m_Animator.SetBool("attack", false);
    }
}
