using UnityEngine;

public class FoodObject : CellObject
{
    public int amountGranted = 10;
    public override void PlayerEnter()
    {
        Destroy(gameObject);

        GameManager.instance.ChangeFood(amountGranted);
    }
}
