using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class PlayerShoot : MonoBehaviour {
    [SerializeField] private float shootCooldown;
    [SerializeField] private Transform shootPosition;

    private float m_shootCooldownTimer;
    private LineRenderer m_lineRender;
    private Vector2 m_mouseLook;

    private void Awake() {
        m_lineRender = GetComponent<LineRenderer>();
    }

    private void Update() {
        m_shootCooldownTimer -= Time.deltaTime;
    }

    public void OnShoot(InputAction.CallbackContext context) {
        if (context.performed) {
            if (m_shootCooldownTimer <= 0) {
                m_shootCooldownTimer = shootCooldown;
                PlayerManager.instance.changePlayerState(PlayerState.Shooting);
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(m_mouseLook);
                if (Physics.Raycast(ray, out hit)) {
                    m_lineRender.SetPosition(0, shootPosition.position);
                    print(hit.point);
                    m_lineRender.SetPosition(1, worldPosition);
                }
            }
        } else if (context.canceled) {
        }
    }

    public void OnMouseLook(InputAction.CallbackContext context) {
        m_mouseLook = context.ReadValue<Vector2>();
    }
}