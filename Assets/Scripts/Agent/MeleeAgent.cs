using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAgent : MonoBehaviour {
    private Agent agent;
    private List<GameObject> enemiesPercibed = new List<GameObject>();
    private MeleeAgentState meleeState;

    // Start is called before the first frame update
    private void Start() {
        agent = GetComponent<Agent>();
    }

    // Update is called once per frame
    private void Update() {
    }

    private void FixedUpdate() {
        perceptionManager();
    }

    private void perceptionManager() {
        Collider[] percibed = Physics.OverlapSphere(agent.getEyePosition(), agent.getEyeRadius());
        foreach (Collider col in percibed) {
            if (col.CompareTag("Enemy")) {
                enemiesPercibed.Add(col.gameObject);
            }
        }
    }

    private void decisonManager() {
        switch (meleeState) {
            case MeleeAgentState.None:
                break;
            case MeleeAgentState.Seeking:
                break;
            case MeleeAgentState.Attacking:
                break;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(agent.getEyePosition(), agent.getEyeRadius());
    }

    public enum MeleeAgentState {
        None,
        Seeking,
        Attacking,
    }
}