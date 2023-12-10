using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    public static PlayerController instance;

    [SerializeField] private float speed;

    private Rigidbody m_rb;
    private Animator m_animator;
    private Vector2 m_movementVector, m_mouseLook;
    private Vector3 m_rotationTarget;

    private void Awake() {
        if (FindObjectOfType<PlayerController>() != null &&
            FindObjectOfType<PlayerController>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start() {
        m_rb = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }

    private void Update() {
        if (PlayerManager.instance.getPlayerState() == PlayerState.Dead) {
            return;
        }
        aim();
    }

    private void FixedUpdate() {
        if (PlayerManager.instance.getPlayerState() == PlayerState.Dead) {
            return;
        }
        horizontalMovement();
    }

    public void OnMove(InputAction.CallbackContext context) {
        m_movementVector = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context) {
        m_mouseLook = context.ReadValue<Vector2>();
    }

    private void aim() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(m_mouseLook);
        if (Physics.Raycast(ray, out hit)) {
            m_rotationTarget = hit.point;
        }
        Vector3 lookPosition = m_rotationTarget - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        Vector3 aimDirection = new Vector3(m_rotationTarget.x, 0f, m_rotationTarget.z);
        if (aimDirection != Vector3.zero) {
            m_rb.rotation = Quaternion.Slerp(m_rb.rotation, rotation, 0.15f);
        }
    }

    private void horizontalMovement() {
        float movementX = m_movementVector.x;
        float movementY = m_movementVector.y;
        m_animator.SetFloat("MoveX", movementX);
        m_animator.SetFloat("MoveY", movementY);
        Vector3 inputMovement = new Vector3(movementX, 0, movementY);
        inputMovement.Normalize();
        Vector3 velocity = inputMovement * speed;
        m_rb.velocity = velocity;
        if (movementX != 0 || movementY != 0) {
            PlayerManager.instance.changePlayerState(PlayerState.Running);
        } else if (inputMovement == Vector3.zero) {
            PlayerManager.instance.changePlayerState(PlayerState.Idle);
        }
    }
}