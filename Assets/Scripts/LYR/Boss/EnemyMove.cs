﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    float moveSpeed = 5f; // 이동 속도
    float rotationSpeed = 720f;
    GameObject nearestObstacle; // 가장 가까운 Obstacle (locked target)
    float searchInterval = 0.5f; // 검색 주기 (초)
    float timer = 0f;
    bool isLockedOnTarget = false; // 타겟 잠금 여부
    float maxDistance = 10f; // 최대 거리 (10 유닛)
    EnemyController _enemyontroller;
    bool hasShot = false;
    bool isShooting = false;
    Rigidbody bossRb;
    Rigidbody obRb;

    public GameObject trash;
    public GameObject ice;
    public GameObject banana;

    [SerializeField]
    Transform trashListObject;
    [SerializeField]
    GameObject player;



    private void Awake()
    {
        trashListObject = FindAnyObjectByType<ObstacleSpawnManager>().gameObject.transform;
        player = FindAnyObjectByType<PlayerMove>().gameObject;
    }
    void Start()
    {
        _enemyontroller = GetComponent<EnemyController>();
        bossRb = GetComponent<Rigidbody>();


        
    }

    void FixedUpdate()
    {
        // 타겟이 없거나, 타겟이 파괴되었거나, 타겟이 10 유닛 이상 멀어졌을 때 새로운 타겟 검색
        if (!isLockedOnTarget || nearestObstacle == null || IsTargetTooFar())
        {
            {
            timer += Time.deltaTime;
            if (timer >= searchInterval)
            {
                FindNearestObstacle();
                timer = 0f;
            }
        }

        // 가장 가까운 Obstacle이 있으면 이동
        if (nearestObstacle != null && !isShooting)
        {
            RotateTowardsObstacle();
            MoveTowardsObstacle();
        }

        if (_enemyontroller.enemyTrashList.Count >= 1 && !hasShot)
        {
            isShooting = true;
            Invoke("ShootTrash", 1f);
            hasShot = true;
        }

        if (_enemyontroller.enemyTrashList.Count == 0)
        {
            hasShot = false;
        }



    }

    bool IsTargetTooFar()
    {
        if (nearestObstacle == null) return true;
        float distance = Vector3.Distance(transform.position, nearestObstacle.transform.position);
        return distance > maxDistance;
    }

    void FindNearestObstacle()
    {
        // Obstacle 태그를 가진 모든 오브젝트 가져오기
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        nearestObstacle = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        // 가장 가까운 Obstacle 찾기 (특정 범위 내에서)
        foreach (GameObject obstacle in obstacles)
        {
            Vector3 obsPos = obstacle.transform.position;
            // 범위 조건: X(-30~30), Y(0 이상), Z(-20~20)
            if (obsPos.x >= -30f && obsPos.x <= 30f &&
                obsPos.y >= 0f && obsPos.y < 2.4f &&
                obsPos.z >= -20f && obsPos.z <= 20f)
            {
                float distance = (obsPos - currentPos).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObstacle = obstacle;
                }
            }
        }

        // 새로운 타겟을 찾았으면 잠금 플래그 설정
        if (nearestObstacle != null)
        {
            isLockedOnTarget = true;
        }
        else
        {
            isLockedOnTarget = false;
        }
    }

    void RotateTowardsObstacle()
    {
        // 목표 방향 계산
        Vector3 direction = (nearestObstacle.transform.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {

            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;

            // 목표 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime / 360f // 회전 속도를 도/초 단위로 변환
            );
        }
    }

    void MoveTowardsObstacle()
    {
        obRb = nearestObstacle.GetComponent<Rigidbody>();

        // 목표 방향으로 이동
        Vector3 targetPos = obRb.position;
        Vector3 moveTarget = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        Vector3 direction = (moveTarget - transform.position).normalized;
        Vector3 velocity = direction * moveSpeed;

        bossRb.MovePosition(transform.position + velocity * Time.deltaTime);

/*        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );*/
    }

    void ShootTrash()
    {
        Vector3 targetDir;

        // 플레이어의 x 위치에 따라 발사 방향 결정
        if (player.transform.position.x <= 0)
        {
            // 플레이어 방향
            targetDir = (player.transform.position - transform.position).normalized;
        }
        else
        {
            // 랜덤 위치 (x = -29, y = 1.67, z = -18~18)
            float randomZ = Random.Range(-18f, 18f);
            Vector3 targetPos = new Vector3(-29f, 1.67f, randomZ);
            targetDir = (targetPos - transform.position).normalized;
        }

        for (int i = 0; i < _enemyontroller.enemyTrashList.Count; i++)
        {
            GameObject shootObj = null;
            int trashId = _enemyontroller.enemyTrashList[i];

            switch (trashId)
            {
                case 1:
                    shootObj = Instantiate(trash, transform.position + Vector3.up * 0.3f * i+ targetDir * 2.5f, Quaternion.identity, trashListObject);
                    shootObj.tag = "Trash";
                    break;
                case 2:
                    shootObj = Instantiate(ice, transform.position + Vector3.up * 0.3f * i+ targetDir * 2.5f, Quaternion.identity, trashListObject);
                    shootObj.tag = "Ice";
                    break;
                case 3:
                    shootObj = Instantiate(banana, transform.position + Vector3.up * 0.3f * i+ targetDir * 2.5f, Quaternion.identity, trashListObject);
                    shootObj.tag = "Banana";
                    break;
            }

            if (shootObj != null)
            {
                Obstacle obs = shootObj.GetComponent<Obstacle>();
                obs.isAttack = true;
                obs.dir = targetDir + Vector3.right * 0.3f * (i - 2);
            }
        }

        _enemyontroller.enemyTrashList.Clear();
        isShooting = false;
    }}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trash") || collision.gameObject.CompareTag("Ice") || collision.gameObject.CompareTag("Banana"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }


}

