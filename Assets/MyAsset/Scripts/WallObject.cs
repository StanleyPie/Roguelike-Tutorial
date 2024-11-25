using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile obstacleTile;
    public int maxHealth = 3;

    protected int health = 0;
    protected Tile m_originalTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        health = maxHealth;

        m_originalTile = GameManager.instance.board.GetCellTile(cell);
        GameManager.instance.board.SetCellTile(cell, obstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        health -= 1;

        if (health > 0)
        {
            return false;
        }

        GameManager.instance.board.SetCellTile(m_Cell, m_originalTile);
        Destroy(gameObject);
        return true;
    }
}
