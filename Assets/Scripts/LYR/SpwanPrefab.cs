using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolingSpawner : MonoBehaviour
{
    public GameObject prefab; // 소환할 프리팹
    public int poolSize = 10; // 풀 크기
    public float spawnRadius = 2f; // 소환 반경
    public float spawnInterval = 5f; // 소환 간격(초)

    private List<GameObject> objectPool; // 오브젝트 풀
    private float timer = 0f;

    void Start()
    {
        // 오브젝트 풀 초기화
        objectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false); // 비활성화 상태로 시작
            objectPool.Add(obj);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f; // 타이머 리셋
        }
    }

    void Spawn()
    {
        // 비활성화된 오브젝트 찾기
        GameObject objToSpawn = GetPooledObject();
        if (objToSpawn != null)
        {
            // 랜덤한 방향으로 위치 계산
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            // 오브젝트 위치 설정 및 활성화
            objToSpawn.transform.position = spawnPosition;
            objToSpawn.transform.rotation = Quaternion.identity;
            objToSpawn.SetActive(true);
        }
    }

    GameObject GetPooledObject()
    {
        // 비활성화된 오브젝트 반환
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        return null; // 풀에 여유 오브젝트가 없으면 null 반환
    }
}