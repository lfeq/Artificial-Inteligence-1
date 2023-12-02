using UnityEngine;

[RequireComponent(typeof(Agent))]
public class BulletAgent : MonoBehaviour {
    private Agent agent;
    private Transform target;
    private Rigidbody rb;

    private void Start() {
        agent = GetComponent<Agent>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rb.velocity = SteeringBehaviours.seek(agent, target.position);
    }

    public void setTarget(Transform t_target) {
        target = t_target;
    }
}