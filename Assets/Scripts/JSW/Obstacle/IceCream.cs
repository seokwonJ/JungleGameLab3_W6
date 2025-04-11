using UnityEngine;

public class IceCream : Obstacle
{
    public override void ChangePlayerState(GameObject collisonPlayer)
    {
        GameObject player = collisonPlayer;
        player.GetComponent<PlayerMove>().ChangetState(trashId);
    }
}
