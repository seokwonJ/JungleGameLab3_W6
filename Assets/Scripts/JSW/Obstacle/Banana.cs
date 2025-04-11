using UnityEngine;

public class Banana : Obstacle
{
    public override void ChangePlayerState(GameObject collisonPlayer)
    {
        GameObject player = collisonPlayer;
        player.GetComponent<PlayerMove>().ChangetState(trashId);
    }
}
