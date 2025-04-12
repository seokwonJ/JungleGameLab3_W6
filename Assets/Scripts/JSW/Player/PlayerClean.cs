using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClean : MonoBehaviour
{
    public ParticleSystem tornado;
    private GameObject _cleanerArea;
    private PlayerInput _playerInput;
    private InputAction _interactAction;
    private PlayerController _playerController;
    

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        // 현재 Action Map에서 Interact 액션을 가져옴
        _interactAction = _playerInput.actions["Interact"];

        _interactAction.performed += OnInteractPerformed;
        _interactAction.canceled += OnInteractCanceled;
    }

    private void Start()
    {
        _cleanerArea = GetComponentInChildren<CleanerArea>().gameObject;
        _cleanerArea.SetActive(false);
    }
    void OnEnable()
    {
        _interactAction?.Enable();
    }

    void OnDisable()
    {
        _interactAction?.Disable();

        // 메모리 누수 방지를 위해 콜백 제거
        _interactAction.performed -= OnInteractPerformed;
        _interactAction.canceled -= OnInteractCanceled;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        _cleanerArea?.SetActive(true);
        tornado.Play();
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        _cleanerArea?.SetActive(false);
        tornado.Stop();
    }
}
