using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    float moveSpeed = 5f; // 이동 속도
    float rotationSpeed = 720f;
    GameObject nearestObstacle; // 가장 가까운 Obstacle (locked target)
    float searchInterval = 0.5f; // 검색 주기 (초)
    float timer = 0f;
    bool isLockedOnTarget = false; // 타겟 잠금 여부
    float maxDistance = 10f; // 최대 거리 (10 유닛)
    BossController _bosscontroller;
    bool hasShot = false;

    public GameObject trash;
    public GameObject ice;
    public GameObject banana;

    public Transform trashListObject;
    public GameObject player;

    void Start()
    {
        _bosscontroller = GetComponent<BossController>();
    }

    void Update()
    {
        // 타겟이 없거나, 타겟이 파괴되었거나, 타겟이 10 유닛 이상 멀어졌을 때 새로운 타겟 검색
        if (!isLockedOnTarget || nearestObstacle == null || IsTargetTooFar())
        {
            timer += Time.deltaTime;
            if (timer >= searchInterval)
            {
                FindNearestObstacle();
                timer = 0f;
            }
        }

        // 가장 가까운 Obstacle이 있으면 이동
        if (nearestObstacle != null)
        {
            RotateTowardsObstacle();
            MoveTowardsObstacle();
        }

        if (_bosscontroller.bossTrashList.Count >= 5 && !hasShot)
        {
            ShootTrash();
            hasShot = true;
        }

        if (_bosscontroller.bossTrashList.Count == 0)
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

        // 가장 가까운 Obstacle 찾기
        foreach (GameObject obstacle in obstacles)
        {
            float distance = (obstacle.transform.position - currentPos).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestObstacle = obstacle;
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
            // 목표 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime / 360f // 회전 속도를 도/초 단위로 변환
            );
        }
    }

    void MoveTowardsObstacle()
    {
        // 목표 방향으로 이동
        Vector3 targetPos = nearestObstacle.transform.position;
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }

    void ShootTrash()
    {
        Vector3 playerDir = (player.transform.position - transform.position).normalized;

        for (int i = 0; i < _bosscontroller.bossTrashList.Count; i++)
        {
            GameObject shootObj = null;
            int trashId = _bosscontroller.bossTrashList[i];

            switch (trashId)
            {
                case 1:
                    shootObj = Instantiate(trash, transform.position + Vector3.up * 0.3f * i, Quaternion.identity, trashListObject);
                    shootObj.tag = "Trash";
                    break;
                case 2:
                    shootObj = Instantiate(ice, transform.position + Vector3.up * 0.3f * i, Quaternion.identity, trashListObject);
                    shootObj.tag = "Ice";
                    break;
                case 3:
                    shootObj = Instantiate(banana, transform.position + Vector3.up * 0.3f * i, Quaternion.identity, trashListObject);
                    shootObj.tag = "Banana";
                    break;
            }

            if (shootObj != null)
            {
                Obstacle obs = shootObj.GetComponent<Obstacle>();
                obs.isAttack = true;
                obs.dir = playerDir + Vector3.right * 0.2f * (i - 2);
            }

        }

        _bosscontroller.bossTrashList.Clear();
    }
}