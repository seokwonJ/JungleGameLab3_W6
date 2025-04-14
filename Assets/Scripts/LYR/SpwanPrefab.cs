using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolingSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    public float spawnInterval = 5f;

    private List<GameObject> objectPool;
    private float timer = 0f;
    private int objectsToSpawnAtOnce = 5;

    void Start()
    {
        objectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnMultiple();
            timer = 0f;
        }
    }

    void SpawnMultiple()
    {
        int availableObjects = 0;
        foreach (var obj in objectPool)
        {
            if (!obj.activeInHierarchy) availableObjects++;
        }

        int spawnCount = Mathf.Min(objectsToSpawnAtOnce, availableObjects);
        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject objToSpawn = GetPooledObject();
            if (objToSpawn != null)
            {
                Vector3 spawnPosition;
                bool validPosition;
                int attempts = 0;
                const int maxAttempts = 10;

                do
                {
                    // x>0, y=1, z=-18~18 범위 내에서 랜덤 위치 생성
                    float x = Random.Range(0f, 20f); // x는 0 이상 (실제로는 적당한 최대값으로 제한 가능)
                    float y = 1f;
                    float z = Random.Range(-18f, 18f);
                    spawnPosition = new Vector3(x, y, z);
                    validPosition = true;

                    // 다른 오브젝트와의 최소 거리 확인
                    foreach (Vector3 pos in usedPositions)
                    {
                        if (Vector3.Distance(spawnPosition, pos) < 1f)
                        {
                            validPosition = false;
                            break;
                        }
                    }
                    attempts++;
                } while (!validPosition && attempts < maxAttempts);

                if (validPosition)
                {
                    objToSpawn.transform.position = spawnPosition;
                    objToSpawn.transform.rotation = Quaternion.identity;
                    objToSpawn.SetActive(true);
                    usedPositions.Add(spawnPosition);
                }
            }
        }
    }

    GameObject GetPooledObject()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        return null;
    }
}