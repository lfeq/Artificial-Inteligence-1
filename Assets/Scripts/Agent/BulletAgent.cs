using UnityEngine;

/// <summary>
/// MeleeAgent class represents a bullet in the game.
/// </summary>
[RequireComponent(typeof(Agent))]
public class BulletAgent : MonoBehaviour {

    #region Private variables

    private Agent m_agent;
    private Transform m_target;
    private Rigidbody m_rb;
    private float m_damage;

    #endregion Private variables

    #region Unity functions

    private void Start() {
        m_agent = GetComponent<Agent>();
        m_rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        m_rb.velocity = SteeringBehaviours.seek(m_agent, m_target.position);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == m_target.gameObject) {
            HealthManager healthManager = other.GetComponent<HealthManager>();
            healthManager.takeDamage(m_damage);
            Destroy(gameObject);
        }
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Sets the target for the agent to pursue or reach.
    /// </summary>
    /// <param name="t_target">The target transform to set.</param>
    public void setTarget(Transform t_target, float t_damage) {
        m_target = t_target;
        m_damage = t_damage;
    }

    #endregion Public functions
}