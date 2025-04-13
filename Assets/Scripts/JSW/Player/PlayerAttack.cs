using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private GameObject _cleanerArea;
    private PlayerInput _playerInput;
    private InputAction _interactAction;

    private PlayerController _playerController;
    private PlayerMove _playMove;
    private float _buttonAttackTime;
    private bool _buttonAttack;

    public GameObject trash;
    public GameObject ice;
    public GameObject banana;
    public Transform trashListObject;
    public ParticleSystem smoke;

    public ParticleSystem Dash_Smoke;


    void Awake()
    {
        Dash_Smoke.Stop();
        _playerInput = GetComponent<PlayerInput>();
        _playMove = GetComponent<PlayerMove>();
        _playerController = GetComponent<PlayerController>();
        _cleanerArea = GetComponentInChildren<CleanerArea>().gameObject;

        // 현재 Action Map에서 Interact 액션을 가져옴
        _interactAction = _playerInput.actions["Attack"];

        _interactAction.performed += OnInteractPerformed;
        _interactAction.canceled += OnInteractCanceled;
    }

    private void Update()
    {
        if (_buttonAttack)
        {
            _buttonAttackTime += Time.deltaTime;
            PlayerLongAttack();
        }
    }

    void OnEnable()
    {
        _interactAction?.Enable();
    }

    void OnDisable()
    {
        _interactAction?.Disable();

        _interactAction.performed -= OnInteractPerformed;
        _interactAction.canceled -= OnInteractCanceled;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        _buttonAttack = true;
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (!_buttonAttack) return;
        PlayerShortAttack();
    }

    private void PlayerShortAttack()
    {
        if (_buttonAttackTime <= 0.2f)
        {
            if (_playerController.trashList.Count > 0)
            {
                InstantiateObstacle(0);

                Camera.main.GetComponent<CameraController>().StartShake(0.2f, 0.05f);
                _playMove.ChangetState(4);
                _playerController.trashList.RemoveAt(0);
                _playerController.Update_Trash();
                smoke.Play();
            }
            else
            {
                Dash_Smoke.Play();
                _playMove.ChangetState(5);
            }
        }
        _buttonAttack = false;
        _buttonAttackTime = 0;
    }

    private void PlayerLongAttack()
    {
        if (_buttonAttackTime > 0.2f)
        {
            //Debug.Log("Attack 취소됨!");
            if (_playerController.trashList.Count > 0)
            {
                for (int i = 0; i < _playerController.trashList.Count; i++)
                {
                    InstantiateObstacle(i);
                }

                Camera.main.GetComponent<CameraController>().StartShake(0.2f, 0.05f);
                _playMove.ChangetState(4);
                _playerController.trashList.Clear();
                _playerController.Update_Trash();
                smoke.Play();
            }
            else
            {
                Dash_Smoke.Play();
                _playMove.ChangetState(5);
            }

            _cleanerArea.SetActive(false);
            _buttonAttack = false;
            _buttonAttackTime = 0;
        }
    }

    public void InstantiateObstacle(int num)
    {
        GameObject shootObject = null;
        switch (_playerController.trashList[num])
        {
            case 1:
                if (num % 2 == 1) num *= -1;
                shootObject = Instantiate(trash, transform.position + transform.forward * 1.4f + transform.right * -1 * 0.05f * num, Quaternion.identity, trashListObject);
                shootObject.tag = "Trash";
                break;
            case 2:
                if (num % 2 == 1) num *= -1;
                shootObject = Instantiate(ice, transform.position + transform.forward * 1.4f + transform.right * -1 * 0.05f * num, Quaternion.identity, trashListObject);
                shootObject.tag = "Ice";
                break;
            case 3:
                if (num % 2 == 1) num *= -1;
                shootObject = Instantiate(banana, transform.position + transform.forward * 1.4f + transform.right * -1 * 0.05f * num, Quaternion.identity, trashListObject);
                shootObject.tag = "Banana";
                break;

            default:
                break;
        }
        Obstacle obstacle = shootObject.GetComponent<Obstacle>();
        obstacle.isAttack = true;
        obstacle.dir = transform.forward;
    }
}
