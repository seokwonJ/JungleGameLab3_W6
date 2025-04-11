using UnityEngine;

public class ButaneGas : Obstacle
{
    float explosionTime = 3f;
    float explosionDeltaTime = 0;
    public float explosionForce = 3000f;
    public float explosionRadius = 10f;
    bool isTimer = false;
    public LayerMask hitTargetLayer = (1 << 3) | (1 << 6);   // 3: Player, 6: Trash


    private void Update()
    {
        if(isTimer)
        {
            explosionDeltaTime += Time.deltaTime;
            if (explosionDeltaTime >= explosionTime)
            {
                isTimer = false;
                Explosion();
            }
        }
    }

    public override void ChangePlayerState(GameObject collisonPlayer)
    {
        GameObject player = collisonPlayer;
        player.GetComponent<PlayerMove>().ChangetState(trashId);

        Explosion();
    }

    void Explosion()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, hitTargetLayer);
        for (int i = 0; i < cols.Length; i++)
        {
            Rigidbody rb = cols[i].GetComponent<Rigidbody>();
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
        gameObject.SetActive(false);
    }

    [ContextMenu("StartTimer")]
    void StartExplosionTimer()
    {
        explosionDeltaTime = 0;
        isTimer = true;
    }
}
