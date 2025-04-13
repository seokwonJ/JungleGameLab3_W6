using UnityEngine;

public class VacuumHit : MonoBehaviour
{
    PlayerMove _playerMove;

    private void Awake()
    {
        _playerMove = GetComponentInParent<PlayerMove>();
    }

    public void PlayerStateChange(int id)
    {
        _playerMove.ChangetState(id);
    }

}
