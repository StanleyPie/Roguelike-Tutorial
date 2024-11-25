using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject_2 : WallObject
{
    public Tile m_breakObj;

    public override bool PlayerWantsToEnter()
    {
        this.health -= 1;

        if (this.health == 1)
        {
            GameManager.instance.board.SetCellTile(m_Cell, m_breakObj);
            return false;
        }

        else if (health > 0)
        {
            return false;
        }

        GameManager.instance.board.SetCellTile(m_Cell, m_originalTile);
        Destroy(gameObject);
        return true;
    }
}
