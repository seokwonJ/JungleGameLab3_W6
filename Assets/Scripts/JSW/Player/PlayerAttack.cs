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

    void Awake()
    {
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
                GameObject shootObject = null;
                switch (_playerController.trashList[0])
                {

                    case 1:
                        shootObject = Instantiate(trash, transform.position + transform.forward * 0.8f + transform.up * 0.2f, Quaternion.identity, trashListObject);
                        shootObject.tag = "Trash";
                        break;
                    case 2:
                        shootObject = Instantiate(ice, transform.position + transform.forward * 0.8f + transform.up * 0.2f, Quaternion.identity, trashListObject);
                        shootObject.tag = "Ice";
                        break;
                    case 3:
                        shootObject = Instantiate(banana, transform.position + transform.forward * 0.8f + transform.up * 0.2f, Quaternion.identity, trashListObject);
                        shootObject.tag = "Banana";
                        break;
                    default:
                        break;
                }
                Obstacle obstacle = shootObject.GetComponent<Obstacle>();
                obstacle.isAttack = true;
                obstacle.dir = transform.forward;

                Camera.main.GetComponent<CameraController>().StartShake(0.2f, 0.03f);
                _playMove.ChangetState(4);
                _playerController.trashList.RemoveAt(0);
            }
            else
            {
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
                    GameObject shootObject = null;
                    switch (_playerController.trashList[i])
                    {

                        case 1:
                            shootObject = Instantiate(trash, transform.position + transform.forward * 0.8f + transform.up * 0.2f * i, Quaternion.identity, trashListObject);
                            shootObject.tag = "Trash";
                            break;
                        case 2:
                            shootObject = Instantiate(ice, transform.position + transform.forward * 0.8f + transform.up * 0.2f * i, Quaternion.identity, trashListObject);
                            shootObject.tag = "Ice";
                            break;
                        case 3:
                            shootObject = Instantiate(banana, transform.position + transform.forward * 0.8f + transform.up * 0.2f * i, Quaternion.identity, trashListObject);
                            shootObject.tag = "Banana";
                            break;
                        default:
                            break;
                    }
                    Obstacle obstacle = shootObject.GetComponent<Obstacle>();
                    obstacle.isAttack = true;
                    obstacle.dir = transform.forward + transform.right * 0.2f * i;
                    //shootObject.transform.GetChild(1).gameObject.SetActive(true);
                }

                Camera.main.GetComponent<CameraController>().StartShake(0.2f, 0.03f);
                _playMove.ChangetState(4);
                _playerController.trashList.Clear();
            }
            else
            {
                _playMove.ChangetState(5);
            }

            _cleanerArea.SetActive(false);
            _buttonAttack = false;
            _buttonAttackTime = 0;
        }
    }
}
