using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;
    public GameObject cameraObj;

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
    }
}