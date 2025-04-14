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

    GameManager manager;

    private void Start()
    {
        manager = FindAnyObjectByType<GameManager>();
    }
    void Update()
    {
        if (playerTransform == null) return;

        timer += Time.deltaTime;
        if (timer >= bombInterval && !manager.isEnd)
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

        yield return new WaitForSeconds(0.5f);

        Destroy(effect);

        for (int i = 0; i < 5; i++)
        {
            Vector2 rand = Random.insideUnitCircle * 1.5f;
            Vector3 spawnPos = center + new Vector3(rand.x, 10f, rand.y);
            GameObject trash = Instantiate(trashPrefab, spawnPos, Quaternion.identity, trashParent);
            trash.tag = "Trash";

            Rigidbody rb = trash.GetComponent<Rigidbody>();

            // Obstacle 컴포넌트 설정
            Obstacle obs = trash.GetComponent<Obstacle>();
            if (obs != null)
            {
                obs.isAttack = true;
                obs.dir = Vector3.down * 0.3f;
            }

            // Y 좌표가 0.5 이하일 때 태그를 Obstacle로 변경
            StartCoroutine(CheckAndChangeTag(trash));
        }
    }

    private IEnumerator CheckAndChangeTag(GameObject trash)
    {
        while (trash != null)
        {
            if (trash.transform.position.y <= 0.5f)
            {
                trash.tag = "Obstacle";
                Obstacle obs = trash.GetComponent<Obstacle>();
                obs.dir = Vector3.zero;

                yield break; // 태그 변경 후 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    public void DropAroundPlayer(Transform playerTransform, float radius = 5f)
    {
        Vector2 randCircle = Random.insideUnitCircle * radius;
        Vector3 targetPos = playerTransform.position + new Vector3(randCircle.x, 0, randCircle.y);

        targetPos.x = Mathf.Clamp(targetPos.x, -25f, 25f);
        targetPos.z = Mathf.Clamp(targetPos.z, -15f, 15f);

        DropTrashAtPosition(targetPos);
    }
}
