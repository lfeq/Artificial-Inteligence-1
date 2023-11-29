using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class MeleeAgent : MonoBehaviour {
    [SerializeField] private float attackRange = 4, attackCooldown = 0.5f;

    private Agent agent;
    private List<GameObject> enemiesPercibed = new List<GameObject>();
    private MeleeAgentState meleeState;
    private Transform target;
    private Rigidbody rb;
    private Animator animator;
    private float attackTimer;

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

    private void perceptionManager() {
        // Sight
        enemiesPercibed.Clear();
        Collider[] percibed = Physics.OverlapSphere(agent.getEyePosition(), agent.getEyeRadius());
        RaycastHit hit;
        foreach (Collider col in percibed) {
            if (col.CompareTag("Enemy")) {
                enemiesPercibed.Add(col.gameObject);
            }
        }
        // Hearing
        percibed = Physics.OverlapSphere(agent.getEarsPosition(), agent.getHearingRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag("Enemy")) {
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
            if (col.CompareTag("Enemy")) {
                enemiesPercibed.Add(col.gameObject);
            }
        }
    }

    private void decisonManager() {
        if (enemiesPercibed.Count <= 0) {
            return;
        }
        int closestEnemy = 0;
        float minDistance = 1000f;
        for (int i = 0; i < enemiesPercibed.Count; i++) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemiesPercibed[i].transform.position);
            if (distanceToEnemy < minDistance) {
                closestEnemy = i;
                minDistance = distanceToEnemy;
            }
        }
        target = enemiesPercibed[closestEnemy].transform;
        if (minDistance > attackRange) {
            meleeState = MeleeAgentState.Seeking;
        } else if (minDistance < attackRange) {
            meleeState = MeleeAgentState.Attacking;
        } else {
            meleeState = MeleeAgentState.None;
        }
        switch (meleeState) {
            case MeleeAgentState.None:
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

    private void movementManager() {
        switch (meleeState) {
            case MeleeAgentState.None:
                animationManager();
                break;
            case MeleeAgentState.Wandering:
                animationManager();
                rb.velocity = SteeringBehavior.wander(agent);
                break;
            case MeleeAgentState.Seeking:
                animationManager();
                rb.velocity = SteeringBehavior.seek(agent, target.position);
                break;
            case MeleeAgentState.Attacking:
                break;
        }
    }

    private void actionManager() {
        switch (meleeState) {
            case MeleeAgentState.None:
                break;
            case MeleeAgentState.Seeking:
                break;
            case MeleeAgentState.Attacking:
                attack();
                break;
        }
    }

    private void animationManager() {
        switch (meleeState) {
            case MeleeAgentState.None:
                animator.SetBool("IsMoving", false);
                break;
            case MeleeAgentState.Seeking:
                animator.SetBool("IsMoving", true);
                break;
            case MeleeAgentState.Attacking:
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

    public enum MeleeAgentState {
        None,
        Seeking,
        Wandering,
        Attacking,
    }
}