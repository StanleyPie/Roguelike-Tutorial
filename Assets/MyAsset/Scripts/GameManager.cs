using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    public BoardManager board;
    public PlayerController player;

    public TurnManager m_turnPlayer {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Start()
    {
        m_turnPlayer = new TurnManager();

        board.GenMap();
        player.Spawn(board, new Vector2Int(1, 1));
    }
}
