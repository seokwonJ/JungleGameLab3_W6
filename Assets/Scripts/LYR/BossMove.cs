using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    float moveSpeed = 5f; // 이동 속도
    float rotationSpeed = 720f;
    GameObject nearestObstacle; // 가장 가까운 Obstacle
    float searchInterval = 0.5f; // 검색 주기 (초)
    float timer = 0f;
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
        // 일정 주기마다 Obstacle 검색
        timer += Time.deltaTime;
        if (timer >= searchInterval)
        {
            FindNearestObstacle();
            timer = 0f;
        }

        // 가장 가까운 Obstacle이 있으면 이동
        if (nearestObstacle != null)
        {
            RotateTowardsObstacle();
            MoveTowardsObstacle();
        }

        if (_bosscontroller.bossTrashList.Count >= 5 && !hasShot)
        {
            Debug.Log("보스가 발사 조건 만족, ShootTrash 호출됨!");
            ShootTrash();
            hasShot = true;
        }

        if (_bosscontroller.bossTrashList.Count == 0)
        {
            hasShot = false;
        }
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
        Debug.Log("💥 ShootTrash() 진입");

        if (_bosscontroller.bossTrashList.Count < 5)
        {
            Debug.Log("❌ 쓰레기 5개 안 모임");
            return;
        }


        if (player == null)
        {
            Debug.Log("❌ 플레이어 찾을 수 없음");
            return;
        }

        Vector3 playerDir = (player.transform.position - transform.position).normalized;

        for (int i = 0; i < _bosscontroller.bossTrashList.Count; i++)
        {
            GameObject shootObj = null;
            int trashId = _bosscontroller.bossTrashList[i];
            Debug.Log($"쓰레기 발사 준비: id={trashId}");

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
                Debug.Log($"🎯 발사 완료: {shootObj.name}");
            }
            else
            {
                Debug.Log("❗ shootObj 생성 실패");
            }
        }

        _bosscontroller.bossTrashList.Clear();
    }

    }
