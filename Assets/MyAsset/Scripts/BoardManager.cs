using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;
    private Grid m_Grid;
    public GameObject cameraObj;
    public PlayerController player;

    public int height;
    public int width;
    public Tile[] groundTiles;
    public Tile[] wallTiles;
    private List<Vector2Int> m_emptyCellList;

    public Vector2Int foodRange;

    //public GameObject foodPrefab;
    public FoodObject[] foodPrefab;


    public class CellData
    {
        public bool passable;
        public CellObject containGameObj;
    }

    private CellData[,] m_boardData;

    public void Awake()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponent<Grid>();
    }

    //public virtual void Update()
    //{
    //   if (Input.GetKeyDown(KeyCode.G))
    //   {
    //        this.GenMap();
    //   }
    //}

    public virtual void GenMap()
    {
        this.cameraObj.transform.position = new Vector3(width / 2, height / 2, -10);

        m_Tilemap.ClearAllTiles();
        m_boardData = new CellData[width, height];
        this.m_emptyCellList = new List<Vector2Int>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tile;
                m_boardData[x, y] = new CellData();

                if (y == 0 || x == 0 || y == height - 1 || x == width - 1)
                {
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    m_boardData[x, y].passable = false;
                }
                else
                {
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                    m_boardData[x, y].passable = true;

                    m_emptyCellList.Add(new Vector2Int(x, y));
                }
                this.m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile); 
            }
        }

        m_emptyCellList.Remove(new Vector2Int(1, 1));
        this.GenFood();
    }

    public virtual Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public virtual CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= width || cellIndex.y < 0  || cellIndex.y >= height)
        {
            return null;
        }
        return m_boardData[cellIndex.x, cellIndex.y];
    }

    void GenFood()
    {
        int foodCount = Random.Range(this.foodRange.x, this.foodRange.y);

        for (int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range(0, this.m_emptyCellList.Count);
            Vector2Int coord = m_emptyCellList[randomIndex];

            m_emptyCellList.RemoveAt(randomIndex);
            CellData data = m_boardData[coord.x, coord.y];

            int randomFood = Random.Range(0, this.foodPrefab.Count());
            FoodObject newFood = Instantiate(this.foodPrefab[randomFood]);

            newFood.transform.position = CellToWorld(coord);
            data.containGameObj = newFood;
        }

    }
}
