using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCleanArea : MonoBehaviour
{
    private float _maxSuctionSpeed = 5f;
    private float _suctionRange = 5f;
    EnemyController _enemyController;
    Transform _EnemyTransform;

    List<Rigidbody> trashList = new List<Rigidbody>(); // 단일 항목만 유지하도록 제한

    void Start()
    {
        _EnemyTransform = transform.parent;
        _enemyController = _EnemyTransform.GetComponent<EnemyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("적 클리너에 장애물 감지됨: " + other.name);
            Rigidbody trashRb = other.attachedRigidbody;
            // 리스트가 비어있을 때만 추가
            if (trashRb != null && trashList.Count == 0)
            {
                trashList.Add(trashRb);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Rigidbody trashRb = other.attachedRigidbody;
            if (trashRb != null && trashList.Contains(trashRb))
            {
                trashList.Remove(trashRb);
            }
        }
    }

    void FixedUpdate()
    {
        if (trashList.Count == 0) return;

        var trashRb = trashList[0]; // 첫 번째 항목만 처리
        if (trashRb == null)
        {
            trashList.Clear();
            return;
        }

        Vector3 direction = (_EnemyTransform.position - trashRb.transform.position);
        float distance = direction.magnitude;

        // 오브젝트 흡수
        if (distance < 3.4f && _enemyController.enemyTrashList.Count < 1)
        {
            _enemyController.enemyTrashList.Add(trashRb.gameObject.GetComponent<Obstacle>().trashId);
            trashList.Clear();
            Destroy(trashRb.gameObject);
            return;
        }

        float t = Mathf.Clamp01(1f - (distance / _suctionRange));
        float suctionSpeed = _maxSuctionSpeed * t;

        trashRb.linearVelocity = direction.normalized * suctionSpeed;
    }
}