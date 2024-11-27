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

    public int startFood = 100;
    private int m_foodAmount;
    public UIDocument uiDoc;
    private Label m_FoodLabel;
    private VisualElement m_VisualElement;
    private Label m_GameOverMEssage;


    private int m_level = 1;

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
        m_FoodLabel.text = "food : " + m_foodAmount;
        m_VisualElement = uiDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMEssage = m_VisualElement.Q<Label>("GameOverMessage");

        m_turnPlayer = new TurnManager();
        m_turnPlayer.OnTick += OnTurnHappen;

        this.StartNewGame();
    }
    
    public void StartNewGame()
    {
        m_VisualElement.style.visibility = Visibility.Hidden;

        m_level = 1;
        m_foodAmount = startFood;
        m_FoodLabel.text = "food : " + m_foodAmount;

        board.CleanUp();
        board.GenMap();
        player.Init();
        player.Spawn(board, new Vector2Int(1, 1));

    }

    public void NewMap()
    {
        board.CleanUp();
        board.GenMap();
        player.Spawn(board, new Vector2Int(1, 1));

        m_level++;
    }


    void OnTurnHappen()
    {
        this.ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        m_foodAmount += amount;
        m_FoodLabel.text = "food : " + m_foodAmount;

        if (m_foodAmount <= 0)
        {
            player.GameOver();
            this.m_VisualElement.style.visibility = Visibility.Visible;
            this.m_GameOverMEssage.text = "Game is over!\n\n you have traveled through: " + m_level + " levels";
        }
    }
}
