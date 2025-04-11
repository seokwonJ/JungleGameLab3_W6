using UnityEngine;

public class Item_SpeedUp : Item
{
    public override void PlayerGetItem(GameObject collisonPlayer)
    {
        GameObject player = collisonPlayer;
        itemManager.Start_SpeedUp(player);
    }
}
