using UnityEngine;

public class Trash : Obstacle
{   
    public override void ChangePlayerState(GameObject collisonPlayer)
    {
        print(collisonPlayer.name);
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
