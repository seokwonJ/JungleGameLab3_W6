using UnityEngine;

public class Item_AttackLimitUp : Item
{
    public override void PlayerGetItem(GameObject collisonPlayer)
    {
        GameObject player = collisonPlayer;
        itemManager.Start_AttackLimitUp(player);
    }
}
