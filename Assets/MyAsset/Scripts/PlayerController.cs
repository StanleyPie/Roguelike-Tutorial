using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_BoardManager;
    private Vector2Int m_CellPosition;

    public virtual void Spawn(BoardManager newBoardManager, Vector2Int newPos)
    {
        m_BoardManager = newBoardManager;
        this.MoveTo(newPos);
    }

    public virtual void MoveTo(Vector2Int newPos)
    {
        this.m_CellPosition = newPos;
        transform.position = m_BoardManager.CellToWorld(newPos);
    }

    public void Update()
    {
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
                this.MoveTo(newCellTarget);
            }
        }
    }
}
