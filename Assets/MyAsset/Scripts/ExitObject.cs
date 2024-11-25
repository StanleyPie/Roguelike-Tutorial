using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitObject : CellObject
{
    public Tile EndTile;

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        GameManager.instance.board.SetCellTile(coord, EndTile);
    }

    public override void PlayerEnter()
    {
        base.PlayerEnter();
        GameManager.instance.NewMap();
    }
}
