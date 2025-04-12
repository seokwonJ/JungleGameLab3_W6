using UnityEngine;

public class PushCubeManager : MonoBehaviour

{
    public GameObject pushCubePrefab;
    public float spawnInterval = 15f;
    public float fixedX = 0f;
    public float fixedY = 1f;
    public float zMin = -4f;
    public float zMax = 4f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnPushCube();
        }
    }

    void SpawnPushCube()
    {
        float randomZ = Random.Range(zMin, zMax);
        Vector3 spawnPos = new Vector3(fixedX, fixedY, randomZ);
        Instantiate(pushCubePrefab, spawnPos, Quaternion.identity);
    }
}


