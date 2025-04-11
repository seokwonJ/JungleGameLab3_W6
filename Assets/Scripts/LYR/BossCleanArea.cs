﻿using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BossCleanArea : MonoBehaviour
{
    private float _maxSuctionSpeed = 4f;
    private float _suctionRange = 3f;
    BossController _bossController;
    Transform _bossTransform;

    List<Rigidbody> trashList = new List<Rigidbody>();

    void Start()
    {
        _bossTransform = transform.parent;
        _bossController = _bossTransform.GetComponent<BossController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("보스 클리너에 장애물 감지됨: " + other.name);
            Rigidbody trashRb = other.attachedRigidbody;
            if ( trashRb != null && !trashList.Contains(trashRb))
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
                trashList.Remove(trashRb);
        }
    }

    void FixedUpdate()
    {

        for (int i = trashList.Count - 1; i >= 0; i--)
        {
            var trashrRB = trashList[i];
            if (trashrRB == null)
            {
                trashList.RemoveAt(i);
                continue;
            }

            Vector3 direction = (_bossTransform.position - trashrRB.transform.position);
            float distance = direction.magnitude;
            if (distance < 3f && _bossController.bossTrashList.Count < 5)
            {
                _bossController.bossTrashList.Add(trashrRB.gameObject.GetComponent<Obstacle>().trashId);
                
                trashList.RemoveAt(i);
                Destroy(trashrRB.gameObject);
                continue;
            }

            float t = Mathf.Clamp01(1f - (distance / _suctionRange));
            float suctionSpeed = _maxSuctionSpeed * t;

            trashrRB.linearVelocity = direction.normalized * suctionSpeed;
        }
    }



}
