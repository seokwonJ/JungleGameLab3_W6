using System.Collections;
using UnityEngine;

public class TrashBombManager : MonoBehaviour
{
    public GameObject trashPrefab;
    public GameObject warningEffectPrefab;
    public Transform trashParent;
    public Transform playerTransform; // ⬅️ 플레이어 참조 필요

    public float bombInterval = 10f;
    private float timer;

    void Update()
    {
        if (playerTransform == null) return;

        timer += Time.deltaTime;
        if (timer >= bombInterval)
        {
            timer = 0f;
            DropAroundPlayer(playerTransform, 5f);
        }
    }

    public void DropTrashAtPosition(Vector3 center)
    {
        StartCoroutine(DelayedDrop(center));
    }

    private IEnumerator DelayedDrop(Vector3 center)
    {
        GameObject effect = Instantiate(warningEffectPrefab, center + Vector3.up * 0.01f, Quaternion.identity);
        effect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        yield return new WaitForSeconds(3f);

        Destroy(effect);

        for (int i = 0; i < 5; i++)
        {
            Vector2 rand = Random.insideUnitCircle * 1.5f;
            Vector3 spawnPos = center + new Vector3(rand.x, 10f, rand.y);
            GameObject trash = Instantiate(trashPrefab, spawnPos, Quaternion.identity, trashParent);
            trash.tag = "Trash";

            Rigidbody rb = trash.GetComponent<Rigidbody>();
            rb.useGravity = false;

            FallingTrash fall = trash.AddComponent<FallingTrash>();
            fall.gravityAccel = 30f;
        }
    }

    public void DropAroundPlayer(Transform playerTransform, float radius = 5f)
    {
        Vector2 randCircle = Random.insideUnitCircle * radius;
        Vector3 targetPos = playerTransform.position + new Vector3(randCircle.x, 0, randCircle.y);
        DropTrashAtPosition(targetPos);
    }
}
