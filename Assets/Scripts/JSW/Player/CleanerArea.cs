using System.Collections.Generic;
using UnityEngine;

public class CleanerArea : MonoBehaviour
{
    private float _maxSuctionSpeed = 4f;
    private float _suctionRange = 3f;
    private PlayerController _playerController;
    private Transform _playerTransform;
    
    private List<Rigidbody> trashInRange = new List<Rigidbody>();

    private void Start()
    {
        _playerTransform = transform.parent;
        _playerController = _playerTransform.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Rigidbody rigidbody = other.attachedRigidbody;
            if (rigidbody != null && !trashInRange.Contains(rigidbody))
                trashInRange.Add(rigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Rigidbody rigidbody = other.attachedRigidbody;
            if (rigidbody != null && trashInRange.Contains(rigidbody))
                trashInRange.Remove(rigidbody);
        }
    }

    private void FixedUpdate()
    {
        for (int i = trashInRange.Count - 1; i >= 0; i--)
        {
            var rigidbody = trashInRange[i];
            if (rigidbody == null)
            {
                trashInRange.RemoveAt(i);
                continue;
            }

            Vector3 direction = (_playerTransform.position - rigidbody.transform.position);
            float distance = direction.magnitude;

            if (distance < 1.5f && _playerController.trashList.Count < 5)
            {
                _playerController.trashList.Add(rigidbody.gameObject.GetComponent<Obstacle>().trashId);
                trashInRange.RemoveAt(i);
                Destroy(rigidbody.gameObject);
                continue;
            }

            float t = Mathf.Clamp01(1f - (distance / _suctionRange));
            float suctionSpeed = _maxSuctionSpeed * t;

            rigidbody.linearVelocity = direction.normalized * suctionSpeed;
        }
    }
}
