using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnManager : MonoBehaviour
{
    public List<GameObject> obstacleList;
    public Transform trashListObject;
    private float _respawnTimeInterval;
    private float _respawnTime;
    private bool _isOverSoon;

    public void Start()
    {
        _respawnTimeInterval = 0.31f;
        _respawnTime = 0;
    }

    private void Update()
    {
        if (_isOverSoon) return;
        SpawnTrash();
    }

    private void SpawnTrash()
    {
        _respawnTime += Time.deltaTime;
        if (_respawnTime > _respawnTimeInterval)
        {
            int randObject = Random.Range(0, 8);
            float posX = Random.Range(-20, 20);
            float posZ = Random.Range(-10, 10);

            Instantiate(obstacleList[randObject], new Vector3(posX, 30, posZ), Quaternion.identity, trashListObject);
            _respawnTime = 0;
        }
    }

    public void SetOverSoon(bool isOverSoon)
    {
        _isOverSoon = isOverSoon;
    }


}
