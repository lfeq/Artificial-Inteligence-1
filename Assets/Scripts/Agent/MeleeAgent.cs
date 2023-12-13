using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MeleeAgent class represents an agent that engages in melee combat.
/// </summary>
[RequireComponent(typeof(Agent))]
public class MeleeAgent : MonoBehaviour {

    #region Serializable variables

    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float attackRange = 4, attackCooldown = 0.5f;
    [SerializeField] private Transform mainTarget;
    [SerializeField, Header("Debugging")] private Color attackRangeColor;
    [SerializeField] private float damage = 5f;

    #endregion Serializable variables

    #region Private variables

    private Agent m_agent;
    private List<GameObject> m_enemiesPercibed = new List<GameObject>();
    private MeleeAgentState m_meleeState;
    private Transform m_target;
    private Rigidbody m_rb;
    private Animator m_animator;
    private float m_attackTimer;
    private bool m_isAttacking;

    #endregion Private variables

    #region Unity functions

    private void Start() {
        m_agent = GetComponent<Agent>();
        m_rb = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
        m_agent.setTargetTransform(PlayerManager.instance.transform);
        m_isAttacking = false;
    }

    private void Update() {
        m_attackTimer -= Time.deltaTime;
    }

    private void FixedUpdate() {
        if (m_isAttacking || m_meleeState == MeleeAgentState.Dead) {
            return;
        }
        perceptionManager();
        decisonManager();
    }

    private void OnDrawGizmos() {
        Gizmos.color = attackRangeColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Inflicts damage to the target during the attack animation.
    /// </summary>
    public void doDamage() {
        PlayerManager.instance.GetComponent<HealthManager>().takeDamage(damage);
    }

    /// <summary>
    /// Allows the agent to continue moving after attack animation is finished.
    /// </summary>
    public void stopAttacking() {
        m_isAttacking = false;
    }

    /// <summary>
    /// Initiates the death sequence for the agent.
    /// </summary>
    public void die() {
        if (m_meleeState == MeleeAgentState.Dead) {
            return;
        }
        m_meleeState = MeleeAgentState.Dead;
        m_animator.SetTrigger("IsDead");
        Destroy(gameObject, 5f);
    }

    #endregion Public functions

    #region Private functions

    /// <summary>
    /// Manages the perception of enemies through sight, hearing, and tact.
    /// </summary>
    private void perceptionManager() {
        // Sight
        m_enemiesPercibed.Clear();
        Collider[] percibed = Physics.OverlapSphere(m_agent.getEyePosition(), m_agent.getEyeRadius());
        RaycastHit hit;
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                m_enemiesPercibed.Add(col.gameObject);
            }
        }
        // Hearing
        percibed = Physics.OverlapSphere(m_agent.getEarsPosition(), m_agent.getHearingRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                Vector3 directionToEnemy = col.transform.position - m_agent.getEarsPosition();
                if (Physics.Raycast(m_agent.getEarsPosition(), directionToEnemy, out hit, m_agent.getHearingRadius())) {
                    if (hit.collider == col) {
                        m_enemiesPercibed.Add(col.gameObject);
                    }
                }
            }
        }
        // Tact
        percibed = Physics.OverlapSphere(m_agent.getTactPosition(), m_agent.getTactRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                m_enemiesPercibed.Add(col.gameObject);
            }
        }
    }

    /// <summary>
    /// Manages AI decision-making based on perceived enemies.
    /// </summary>
    private void decisonManager() {
        int closestEnemy = 0;
        float minDistance = 1000f;
        if (m_enemiesPercibed.Count > 0) {
            for (int i = 0; i < m_enemiesPercibed.Count; i++) {
                float distanceToEnemy = Vector3.Distance(transform.position, m_enemiesPercibed[i].transform.position);
                if (distanceToEnemy < minDistance) {
                    closestEnemy = i;
                    minDistance = distanceToEnemy;
                }
            }
            m_target = m_enemiesPercibed[closestEnemy].transform;
        }
        if (minDistance > attackRange) {
            m_meleeState = MeleeAgentState.Seeking;
        }
        if (minDistance < attackRange) {
            m_meleeState = MeleeAgentState.Attacking;
        }
        if (m_enemiesPercibed.Count == 0) {
            m_meleeState = MeleeAgentState.Seeking;
            m_target = mainTarget;
        }
        switch (m_meleeState) {
            case MeleeAgentState.None:
                movementManager();
                break;
            case MeleeAgentState.Wandering:
                movementManager();
                break;
            case MeleeAgentState.Seeking:
                movementManager();
                break;
            case MeleeAgentState.Attacking:
                actionManager();
                break;
        }
    }

    /// <summary>
    /// Manages movement based on the current AI state.
    /// </summary>
    private void movementManager() {
        switch (m_meleeState) {
            case MeleeAgentState.None:
                animationManager();
                break;
            case MeleeAgentState.Wandering:
                animationManager();
                m_rb.velocity = SteeringBehaviours.wander(m_agent);
                break;
            case MeleeAgentState.Seeking:
                animationManager();
                m_rb.velocity = SteeringBehaviours.pathFollowing(m_agent);
                transform.LookAt(m_agent.getTargetTranform().position);
                break;
            case MeleeAgentState.Attacking:
                break;
        }
    }

    /// <summary>
    /// Manages actions based on the current AI state.
    /// </summary>
    private void actionManager() {
        switch (m_meleeState) {
            case MeleeAgentState.None:
                break;
            case MeleeAgentState.Seeking:
                break;
            case MeleeAgentState.Attacking:
                attack();
                break;
        }
    }

    /// <summary>
    /// Manages animations based on the current AI state.
    /// </summary>
    private void animationManager() {
        switch (m_meleeState) {
            case MeleeAgentState.None:
                m_animator.SetBool("IsMoving", false);
                break;
            case MeleeAgentState.Seeking:
                m_animator.SetBool("IsMoving", true);
                break;
            case MeleeAgentState.Attacking:
                m_animator.SetTrigger("Attack");
                break;
        }
    }

    /// <summary>
    /// Initiates an attack if the attack timer allows it.
    /// </summary>
    private void attack() {
        if (m_attackTimer > 0) {
            return;
        }
        m_attackTimer = attackCooldown;
        m_rb.velocity = Vector3.zero;
        m_isAttacking = true;
        transform.LookAt(m_agent.getTargetTranform().position);
        animationManager();
    }

    #endregion Private functions

    #region Enums

    /// <summary>
    /// Enumeration representing different states of the MeleeAgent.
    /// </summary>
    public enum MeleeAgentState {
        None,
        Seeking,
        Wandering,
        Attacking,
        Dead
    }

    #endregion Enums
}