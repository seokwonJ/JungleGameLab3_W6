using UnityEngine;

public class IceCream : Obstacle
{
    public override void ChangePlayerState(GameObject collisonPlayer)
    {
        GameObject player = collisonPlayer;
        if (player.GetComponent<PlayerMove>() != null)
        {
            player.GetComponent<PlayerMove>().ChangetState(trashId);
        }
        else
        {
            player.GetComponent<VacuumHit>().PlayerStateChange(trashId);
        }
    }
}
