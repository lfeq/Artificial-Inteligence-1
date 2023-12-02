using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static MeleeAgent;

[RequireComponent(typeof(Agent))]
public class RangeAgent : MonoBehaviour {
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float attackRange = 4, attackCooldown = 0.5f;
    [SerializeField, Header("Debugging")] private Color attackRangeColor = Color.white;
    [SerializeField] private Transform mainTarget;

    private Agent agent;
    private List<GameObject> enemiesPercibed = new List<GameObject>();
    private RangeAgentState rangeState;
    private Transform target;
    private Rigidbody rb;
    private Animator animator;
    private float attackTimer;

    #region Unity Functions

    private void Start() {
        agent = GetComponent<Agent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        attackTimer -= Time.deltaTime;
    }

    private void FixedUpdate() {
        perceptionManager();
        decisonManager();
    }

    private void OnDrawGizmos() {
        Gizmos.color = attackRangeColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    #endregion Unity Functions

    #region Class Private Functions

    private void perceptionManager() {
        // Sight
        enemiesPercibed.Clear();
        Collider[] percibed = Physics.OverlapSphere(agent.getEyePosition(), agent.getEyeRadius());
        RaycastHit hit;
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                enemiesPercibed.Add(col.gameObject);
            }
        }
        // Hearing
        percibed = Physics.OverlapSphere(agent.getEarsPosition(), agent.getHearingRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                Vector3 directionToEnemy = col.transform.position - agent.getEarsPosition();
                if (Physics.Raycast(agent.getEarsPosition(), directionToEnemy, out hit, agent.getHearingRadius())) {
                    if (hit.collider == col) {
                        enemiesPercibed.Add(col.gameObject);
                    }
                }
            }
        }
        // Tact
        percibed = Physics.OverlapSphere(agent.getTactPosition(), agent.getTactRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag(enemyTag)) {
                enemiesPercibed.Add(col.gameObject);
            }
        }
    }

    private void decisonManager() {
        int closestEnemy = 0;
        float minDistance = 1000f;
        if (enemiesPercibed.Count > 0) {
            for (int i = 0; i < enemiesPercibed.Count; i++) {
                float distanceToEnemy = Vector3.Distance(transform.position, enemiesPercibed[i].transform.position);
                if (distanceToEnemy < minDistance) {
                    closestEnemy = i;
                    minDistance = distanceToEnemy;
                }
            }
            target = enemiesPercibed[closestEnemy].transform;
        }
        if (minDistance > attackRange) {
            rangeState = RangeAgentState.Seeking;
        }
        if (minDistance < attackRange) {
            rangeState = RangeAgentState.Attacking;
        }
        if (enemiesPercibed.Count == 0) {
            rangeState = RangeAgentState.Seeking;
            target = mainTarget;
        }
        switch (rangeState) {
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

    private void movementManager() {
        switch (rangeState) {
            case RangeAgentState.None:
                animationManager();
                break;
            case RangeAgentState.Wandering:
                animationManager();
                rb.velocity = SteeringBehaviours.wander(agent);
                break;
            case RangeAgentState.Seeking:
                animationManager();
                rb.velocity = SteeringBehaviours.seek(agent, target.position);
                transform.LookAt(target.position);
                break;
            case RangeAgentState.Attacking:
                break;
        }
    }

    private void actionManager() {
        switch (rangeState) {
            case RangeAgentState.None:
                break;
            case RangeAgentState.Seeking:
                break;
            case RangeAgentState.Attacking:
                attack();
                break;
        }
    }

    private void animationManager() {
        switch (rangeState) {
            case RangeAgentState.None:
                animator.SetBool("IsMoving", false);
                break;
            case RangeAgentState.Seeking:
                animator.SetBool("IsMoving", true);
                break;
            case RangeAgentState.Attacking:
                animator.SetTrigger("Attack");
                break;
        }
    }

    private void attack() {
        if (attackTimer > 0) {
            return;
        }
        attackTimer = attackCooldown;
        animationManager();
    }

    #endregion Class Private Functions

    public enum RangeAgentState {
        None,
        Seeking,
        Wandering,
        Attacking,
    }
}