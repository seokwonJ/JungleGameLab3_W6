using System.Collections.Generic;
using UnityEngine;

public class CleanerArea : MonoBehaviour
{
    private float _maxSuctionSpeed = 10f;
    private float _suctionRange = 7f;
    private PlayerController _playerController;
    private Transform _playerTransform;
    
    private List<Rigidbody> _trashInRange = new List<Rigidbody>();
    private int _attackLimit = 5;

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
            if (rigidbody != null && !_trashInRange.Contains(rigidbody))
                _trashInRange.Add(rigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Rigidbody rigidbody = other.attachedRigidbody;
            if (rigidbody != null && _trashInRange.Contains(rigidbody))
                _trashInRange.Remove(rigidbody);
        }
    }

    private void FixedUpdate()
    {
        for (int i = _trashInRange.Count - 1; i >= 0; i--)
        {
            var rigidbody = _trashInRange[i];
            if (rigidbody == null)
            {
                _trashInRange.RemoveAt(i);
                continue;
            }

            Vector3 direction = (_playerTransform.position - rigidbody.transform.position);
            float distance = direction.magnitude;

            if (distance < 3f && _playerController.trashList.Count < _attackLimit)
            {
                _playerController.trashList.Add(rigidbody.gameObject.GetComponent<Obstacle>().trashId);
                _trashInRange.RemoveAt(i);
                Destroy(rigidbody.gameObject);
                continue;
            }

            float t = Mathf.Clamp01(1f - (distance / _suctionRange));
            float suctionSpeed = _maxSuctionSpeed * t;

            rigidbody.linearVelocity = direction.normalized * suctionSpeed;
        }
    }

    public void SetAttackLimit(int num)
    {
        _attackLimit = num;
    }

    public int GetAttackLimit()
    {
        return _attackLimit;
    }
}
