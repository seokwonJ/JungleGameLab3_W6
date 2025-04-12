using UnityEngine;

public class FallingTrash : MonoBehaviour
{
    public float gravityAccel = 30f;
    private Rigidbody rb;
    private bool hasLanded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.down * 2f; // 초기 속도
    }

    void FixedUpdate()
    {
        if (!hasLanded)
        {
            rb.linearVelocity += Vector3.down * gravityAccel * Time.fixedDeltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasLanded)
        {
            hasLanded = true;
            tag = "Obstacle";
            rb.useGravity = true;
            Destroy(this); // 컴포넌트 제거
        }
    }
}
