using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayerShoot class handles shooting mechanics for the player.
/// </summary>
public class PlayerShoot : MonoBehaviour {

    #region Serializable variables

    [SerializeField] private float shootCooldown;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float shootRange = 15;
    [SerializeField] private float damage = 30f;
    [SerializeField, Header("Audio")] private AudioClip shootSound;
    [SerializeField, Range(0f, 1f)] private float audioVolume = 0.5f;

    #endregion Serializable variables

    #region Private variables

    private float m_shootCooldownTimer;
    private Vector2 m_mouseLook;
    private AudioSource m_audioSource;

    #endregion Private variables

    #region Unity functions

    private void Start() {
        m_audioSource = transform.AddComponent<AudioSource>();
        m_audioSource.volume = audioVolume;
        m_audioSource.playOnAwake = false;
        m_audioSource.loop = false;
        m_audioSource.clip = shootSound;
    }

    private void Update() {
        m_shootCooldownTimer -= Time.deltaTime;
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Called when the player performs a shoot action.
    /// </summary>
    /// <param name="context">The input context.</param>
    public void OnShoot(InputAction.CallbackContext context) {
        if (context.performed) {
            if (m_shootCooldownTimer <= 0) {
                m_audioSource.Play();
                m_shootCooldownTimer = shootCooldown;
                PlayerManager.instance.changePlayerState(PlayerState.Shooting);
                RaycastHit hit;
                if (Physics.Raycast(shootPosition.position, transform.TransformDirection(Vector3.forward), out hit, shootRange)) {
                    if (hit.transform.TryGetComponent<HealthManager>(out HealthManager health)) {
                        health.takeDamage(damage);
                    }
                    if (hit.transform.CompareTag("Bullet")) {
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
        } else if (context.canceled) {
        }
    }

    /// <summary>
    /// Called when the player performs a mouse look action.
    /// </summary>
    /// <param name="context">The input context.</param>
    public void OnMouseLook(InputAction.CallbackContext context) {
        m_mouseLook = context.ReadValue<Vector2>();
    }

    #endregion Public functions
}