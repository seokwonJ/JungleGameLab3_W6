using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int trashId;
    public int speed;
    public Vector3 dir; 
    public bool isAttack;
    public ParticleSystem hitParticle;

    private Rigidbody _rigidBody;
    private bool _isRight;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (transform.position.x > 0)
        {
            _isRight = true;
        }
        if (GameManager.Instance == null) return;
        GameManager.Instance.UpdateScore(_isRight, 1);
    }

    void FixedUpdate()
    {
        if (isAttack)
        {
            _rigidBody.linearVelocity = dir.normalized * speed;
        }

        if (GameManager.Instance == null) return;

        if (_isRight && transform.position.x < 0)
        {
            GameManager.Instance.UpdateScore(_isRight, -1);
            _isRight = false;
            GameManager.Instance.UpdateScore(_isRight, 1);
        }

        if (!_isRight && transform.position.x > 0)
        {
            GameManager.Instance.UpdateScore(_isRight, -1);
            _isRight = true;
            GameManager.Instance.UpdateScore(_isRight, 1);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("GiantArea")) && transform.tag != "Obstacle")
        {

            ContactPoint contact = collision.GetContact(0);
            Vector3 reflectDir = Vector3.Reflect(dir.normalized, contact.normal);
            Vector3 bounce = reflectDir * 3f; // 튕김 강도
            hitParticle.transform.position = contact.point;
            hitParticle.Play();

            // 튕기는 효과 적용
            if (_rigidBody != null)
            {
                _rigidBody.linearVelocity = Vector3.zero;
                _rigidBody.AddForce(bounce, ForceMode.Impulse);
            }

            // 플레이어와 충돌한 경우 추가 처리
            if (collision.gameObject.tag == "Player")
            {
                ChangePlayerState(collision.gameObject);
            }

            // 튕김 상태와 이동 상태 전환
            isAttack = false;
            transform.tag = "Obstacle";

            //transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.UpdateScore(_isRight, -1);
    }

    public virtual void ChangePlayerState(GameObject collisonPlayer)
    {
        Debug.Log("Hello from Parent!");
    }
}
