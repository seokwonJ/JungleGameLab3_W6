using UnityEngine;

public class PushCube : MonoBehaviour
{
    public float moveDistance = 10f;
    public float moveSpeed = 5f;

    private Vector3 startPos;
    private Vector3 targetPos;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.left * moveDistance;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // 목표 위치에 도달하면 제거
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody != null && other.gameObject.layer != LayerMask.NameToLayer("MapBoundary"))
        {
            Vector3 pushDir = Vector3.left;
            other.attachedRigidbody.AddForce(pushDir * 10f, ForceMode.Force);
        }
    }
}
