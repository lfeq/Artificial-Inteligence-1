using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class MeleeAgent : MonoBehaviour {
    [SerializeField] private float attackRange = 4;

    private Agent agent;
    private List<GameObject> enemiesPercibed = new List<GameObject>();
    private MeleeAgentState meleeState;
    private Transform target;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start() {
        agent = GetComponent<Agent>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        perceptionManager();
        decisonManager();
    }

    private void perceptionManager() {
        enemiesPercibed.Clear();
        Collider[] percibed = Physics.OverlapSphere(agent.getEyePosition(), agent.getEyeRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag("Enemy")) {
                enemiesPercibed.Add(col.gameObject);
            }
        }
    }

    private void decisonManager() {
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
            meleeState = MeleeAgentState.Wandering;
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
                break;
            case MeleeAgentState.Wandering:
                rb.velocity = SteeringBehavior.wander(agent);
                break;
            case MeleeAgentState.Seeking:
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
                Debug.Log("Attack");
                break;
        }
    }

    public enum MeleeAgentState {
        None,
        Seeking,
        Wandering,
        Attacking,
    }
}