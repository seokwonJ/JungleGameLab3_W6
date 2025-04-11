using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnManager : MonoBehaviour
{
    public List<GameObject> obstacleList;
    public Transform trashListObject;
    private float _respawnTimeInterval;
    private float _respawnTime;
    

    public void Start()
    {
        _respawnTimeInterval = 1;
        _respawnTime = 0;
    }

    private void Update()
    {
        _respawnTime += Time.deltaTime;
        if (_respawnTime > _respawnTimeInterval)
        {
            int randObject = Random.Range(0, 3);
            float posX = Random.Range(-9, 9);
            float posZ = Random.Range(-4, 4);

            Instantiate(obstacleList[randObject], new Vector3(posX, 1, posZ), Quaternion.identity, trashListObject);
            _respawnTime = 0;
        }
    }
}
