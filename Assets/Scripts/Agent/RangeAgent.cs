using System.Collections.Generic;
using UnityEngine;
using static MeleeAgent;

/// <summary>
/// RangeAgent class represents an agent that engages in range combat.
/// </summary>
[RequireComponent(typeof(Agent))]
public class RangeAgent : MonoBehaviour {

    #region Serializable variables

    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float attackRange = 4, attackCooldown = 0.5f;
    [SerializeField] private Transform mainTarget;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField, Header("Debugging")] private Color attackRangeColor = Color.white;
    [SerializeField] private float damage = 10f;

    #endregion Serializable variables

    #region Private variables

    private Agent m_agent;
    private List<GameObject> m_enemiesPercibed = new List<GameObject>();
    private RangeAgentState m_rangeState;
    private Transform m_target;
    private Rigidbody m_rb;
    private Animator m_animator;
    private float m_attackTimer;
    private bool m_isAttacking;

    #endregion Private variables

    #region Unity Functions

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
        if (m_isAttacking || m_rangeState == RangeAgentState.Dead) {
            return;
        }
        perceptionManager();
        decisonManager();
    }

    private void OnDrawGizmos() {
        Gizmos.color = attackRangeColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    #endregion Unity Functions

    #region Public functions

    /// <summary>
    /// Spawns a bullet agent at the specified position and sets its target.
    /// </summary>
    public void spawnBullet() {
        BulletAgent tempBullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity).GetComponent<BulletAgent>();
        tempBullet.setTarget(m_target, damage);
    }

    /// <summary>
    /// Allows the agent to continue moving after attack animation is finished.
    /// </summary>
    public void stopIsAttacking() {
        m_isAttacking = false;
    }

    /// <summary>
    /// Initiates the death sequence for the agent.
    /// </summary>
    public void die() {
        if (m_rangeState == RangeAgentState.Dead) {
            return;
        }
        m_rangeState = RangeAgentState.Dead;
        m_animator.SetTrigger("IsDead");
        Destroy(gameObject, 5f);
    }

    #endregion Public functions

    #region Class Private Functions

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
            m_rangeState = RangeAgentState.Seeking;
        }
        if (minDistance < attackRange) {
            m_rangeState = RangeAgentState.Attacking;
        }
        if (m_enemiesPercibed.Count == 0) {
            m_rangeState = RangeAgentState.Seeking;
            m_target = mainTarget;
        }
        switch (m_rangeState) {
            case RangeAgentState.None:
                movementManager();
                break;
            case RangeAgentState.Wandering:
                movementManager();
                break;
            case RangeAgentState.Seeking:
                movementManager();
                break;
            case RangeAgentState.Attacking:
                actionManager();
                break;
        }
    }

    /// <summary>
    /// Manages movement based on the current AI state.
    /// </summary>
    private void movementManager() {
        switch (m_rangeState) {
            case RangeAgentState.None:
                animationManager();
                break;
            case RangeAgentState.Wandering:
                animationManager();
                m_rb.velocity = SteeringBehaviours.wander(m_agent);
                break;
            case RangeAgentState.Seeking:
                animationManager();
                m_rb.velocity = SteeringBehaviours.pathFollowing(m_agent);
                transform.LookAt(m_agent.getTargetTranform().position);
                break;
            case RangeAgentState.Attacking:
                break;
        }
    }

    /// <summary>
    /// Manages actions based on the current AI state.
    /// </summary>
    private void actionManager() {
        switch (m_rangeState) {
            case RangeAgentState.None:
                break;
            case RangeAgentState.Seeking:
                break;
            case RangeAgentState.Attacking:
                attack();
                break;
        }
    }

    /// <summary>
    /// Manages animations based on the current AI state.
    /// </summary>
    private void animationManager() {
        switch (m_rangeState) {
            case RangeAgentState.None:
                m_animator.SetBool("IsMoving", false);
                break;
            case RangeAgentState.Seeking:
                m_animator.SetBool("IsMoving", true);
                break;
            case RangeAgentState.Attacking:
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

    #endregion Class Private Functions

    #region Enums

    /// <summary>
    /// Enumeration representing different states of the RangeAgent.
    /// </summary>
    public enum RangeAgentState {
        None,
        Seeking,
        Wandering,
        Attacking,
        Dead
    }

    #endregion Enums
}