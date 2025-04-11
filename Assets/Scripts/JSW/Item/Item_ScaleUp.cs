using UnityEngine;

public class Item_ScaleUp : Item
{
    public override void PlayerGetItem(GameObject collisonPlayer)
    {
        GameObject player = collisonPlayer;
        itemManager.Start_ScaleUp(player);
    }
}
