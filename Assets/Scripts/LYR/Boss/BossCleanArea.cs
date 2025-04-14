using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BossCleanArea : MonoBehaviour
{
    private float _maxSuctionSpeed = 15f;
    private float _suctionRange = 10f;
    BossController _bossController;
    Transform _bossTransform;

    List<Rigidbody> trashList = new List<Rigidbody>();

    public int maxTrash = 5;

    void Start()
    {
        _bossTransform = transform.parent;
        _bossController = _bossTransform.GetComponent<BossController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            //Debug.Log("보스 클리너에 장애물 감지됨: " + other.name);
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
            //오브젝트 흡수
            if (distance < 3.5f && _bossController.bossTrashList.Count < maxTrash)
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
