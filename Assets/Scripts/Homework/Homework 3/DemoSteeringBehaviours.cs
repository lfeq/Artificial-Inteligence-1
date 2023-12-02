using UnityEngine;

[RequireComponent(typeof(Agent))]
public class DemoSteeringBehaviours : MonoBehaviour {
    [SerializeField] private SteeringBehaviourState state;
    [SerializeField] private GameObject target;
    [SerializeField] private Agent pursuitTarget;

    private Agent agent;
    private Rigidbody rb;

    private void Start() {
        agent = GetComponent<Agent>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        switch (state) {
            case SteeringBehaviourState.seek:
                rb.velocity = SteeringBehaviours.seek(agent, target.transform.position);
                break;
            case SteeringBehaviourState.flee:
                rb.velocity = SteeringBehaviours.flee(agent, target.transform.position);
                break;
            case SteeringBehaviourState.followPath:
                rb.velocity = SteeringBehaviours.pathFollowing(agent);
                break;
            case SteeringBehaviourState.pursuit:
                rb.velocity = SteeringBehaviours.pursuit(agent, pursuitTarget);
                break;
        }
    }

    public enum SteeringBehaviourState {
        seek,
        flee,
        followPath,
        pursuit,
    }
}