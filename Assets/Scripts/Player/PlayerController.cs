using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayerController class handles player input, character movement, and aiming.
/// </summary>
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    #region Public variables

    public static PlayerController instance;

    #endregion Public variables

    #region Serializable variables

    [SerializeField] private float speed;
    [SerializeField] private AudioClip dieSound;
    [SerializeField, Range(0, 1)] private float audioVolume = 0.5f;

    #endregion Serializable variables

    #region Private variables

    private Rigidbody m_rb;
    private Animator m_animator;
    private Vector2 m_movementVector, m_mouseLook;
    private Vector3 m_rotationTarget;
    private AudioSource m_audioSource;

    #endregion Private variables

    #region Unity functions

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
        m_audioSource = transform.AddComponent<AudioSource>();
        m_audioSource.loop = false;
        m_audioSource.clip = dieSound;
        m_audioSource.volume = audioVolume;
        m_audioSource.playOnAwake = false;
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

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Called when the player moves using input.
    /// </summary>
    /// <param name="context">The input context.</param>
    public void OnMove(InputAction.CallbackContext context) {
        m_movementVector = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Called when the player performs a mouse look.
    /// </summary>
    /// <param name="context">The input context.</param>
    public void OnMouseLook(InputAction.CallbackContext context) {
        m_mouseLook = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Initiates the death of the player character.
    /// </summary>
    public void die() {
        if (PlayerManager.instance.getPlayerState() == PlayerState.Dead) {
            return;
        }
        PlayerManager.instance.changePlayerState(PlayerState.Dead);
        m_animator.SetTrigger("die");
        LevelManager.instance.showGameOverScreen();
        m_audioSource.Play();
    }

    #endregion Public functions

    #region Private functions

    /// <summary>
    /// Handles aiming based on mouse position.
    /// </summary>
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

    /// <summary>
    /// Handles horizontal movement based on player input.
    /// </summary>
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

    #endregion Private functions
}