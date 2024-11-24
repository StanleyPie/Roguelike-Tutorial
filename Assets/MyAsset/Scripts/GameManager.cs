using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    public BoardManager board;
    public PlayerController player;
    public TurnManager m_turnPlayer {  get; private set; }

    private int m_FoodAmount = 100;
    public UIDocument uiDoc;
    private Label m_FoodLabel;

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
        this.SetUp();
    }

    private void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            this.SetUp();
        }
    }


    void SetUp()
    {
        m_FoodLabel = uiDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "food : " + m_FoodAmount;

        m_turnPlayer = new TurnManager();
        m_turnPlayer.OnTick += OnTurnHappen;

        board.GenMap();
        player.Spawn(board, new Vector2Int(1, 1));
    }


    void OnTurnHappen()
    {
        this.ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = "food : " + m_FoodAmount;
    }
}
