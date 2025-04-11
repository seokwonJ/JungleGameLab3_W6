using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Vector3 inputVec;
    public float speed;
    public float slowSpeed;
    private float _nowSpeed;
    
    Rigidbody rigid;

    private int _numState = 0;
    private float _stateTime = 0;
    private float _stunTime = 1f;
    private float _iceTime = 3f;
    private float _bananaTime = 0.7f;
    private float _attackTime = 0.1f;
    private float _dashTime = 0.4f;
    private Vector3 _lastDir;
    private Animator _animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        // 기본 상태
        if (_numState == 0)
        {
            _nowSpeed = speed;
        }
        // 잠깐 스턴 및 아이템 떨구기 (action으로 invoke하면 좋을 듯)
        else if (_numState == 1)
        {
            _stateTime += Time.deltaTime;
            if (_stateTime > _stunTime)
            {
                ChangetState(0);
            }
            else
            {
                _nowSpeed = 0;
            }
        }
        // 일정시간 느려지기
        else if (_numState == 2)
        {
            _stateTime += Time.deltaTime;
            if (_stateTime > _iceTime)
            {
                ChangetState(0); _numState = 0;
            }
            else
            {
                _nowSpeed = slowSpeed;
            }
        }
        // 미끄러지기
        else if (_numState == 3)
        {
            _stateTime += Time.deltaTime;
            if (_stateTime > _bananaTime)
            {
                ChangetState(0);
            }
            else
            {
                _nowSpeed = 30;
            }
        }
        // 공격 반동
        else if (_numState == 4)
        {
            _stateTime += Time.deltaTime;
            if (_stateTime > _attackTime)
            {
                ChangetState(0);
                inputVec = _lastDir;
            }
            else
            {
                inputVec = transform.forward * -1;
                _nowSpeed = Mathf.Lerp(_nowSpeed, speed, Time.deltaTime * 30);
            }
        }
        // 대쉬
        else if (_numState == 5)
        {
            _stateTime += Time.deltaTime;
            if (_stateTime > _dashTime)
            {
                ChangetState(0);
                inputVec = _lastDir;
            }
            else
            {
                inputVec = transform.forward * -1;
                _nowSpeed = Mathf.Lerp(_nowSpeed, speed, Time.deltaTime * 8);
                if (_nowSpeed - speed < 1f)
                {
                    _stateTime = _dashTime;
                }
            }
        }

        Vector3 moveDir = new Vector3(inputVec.x, 0, inputVec.z);
        Vector3 nextVec = moveDir.normalized * _nowSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        // 회전
        if (moveDir != Vector3.zero && (_numState != 4 && _numState != 5))
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        inputVec = new Vector3(input.x, 0, input.y); // X,Z로 이동

        _animator.SetFloat("moveDirection_x", input.x * 2f);
        _animator.SetFloat("moveDirection_y", input.y * 2f); // z축이 전후 이동에 해당

        if (_numState == 4 || _numState == 5)
        {
            _lastDir = inputVec;
        }
        else if (_numState != 3)
        {
        }
    }



    public void ChangetState(int num)
    {
        if (_numState == 5 && num == 5 && _stateTime > _dashTime - 0.1)
        {
            _nowSpeed = 60;
            _stateTime = 0;
        }
        if (_numState == num) return;
        if (_numState >= 1 && _numState <= 3 && (num == 4 || num == 5)) return;
        _stateTime = 0;
        _numState = num;
        if (_numState == 4 || _numState == 5)
        {
            _lastDir = inputVec;
            _nowSpeed = 60;
        }
    }
}
