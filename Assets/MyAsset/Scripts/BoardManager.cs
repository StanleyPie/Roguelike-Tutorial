using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public class CellData
    {
        public bool passable;
    }

    private CellData[,] m_boardData;

    public void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponent<Grid>();
    }

    public virtual void Update()
    {
       if (Input.GetKeyDown(KeyCode.G))
       {
            this.GenMap();
       }
    }

    public virtual void GenMap()
    {
        this.cameraObj.transform.position = new Vector3(width / 2, height / 2, -10);

        m_Tilemap.ClearAllTiles();
        m_boardData = new CellData[width, height];

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
                }

                int tileNumber = Random.Range(0, groundTiles.Length);
                this.m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile); 
            }
        }

        this.player.Spawn(this, new Vector2Int(1, 1));
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
}
