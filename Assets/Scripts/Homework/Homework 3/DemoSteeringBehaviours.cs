using UnityEngine;

[RequireComponent(typeof(Agent))]
public class DemoSteeringBehaviours : MonoBehaviour {

    #region Serializable variables

    [SerializeField] private SteeringBehaviourState state;
    [SerializeField] private GameObject target;
    [SerializeField] private Agent pursuitTarget;

    #endregion Serializable variables

    #region Private variables

    private Agent agent;
    private Rigidbody rb;

    #endregion Private variables

    #region Unity functions

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
            case SteeringBehaviourState.evade:
                rb.velocity = SteeringBehaviours.evade(agent, pursuitTarget);
                break;
            case SteeringBehaviourState.followLeader:
                rb.velocity = SteeringBehaviours.followLeader(agent, pursuitTarget);
                break;
            case SteeringBehaviourState.queue:
                rb.velocity = SteeringBehaviours.seek(agent, target.transform.position);
                SteeringBehaviours.queue(agent);
                break;
        }
    }

    #endregion Unity functions

    #region Enums

    public enum SteeringBehaviourState {
        seek,
        flee,
        followPath,
        pursuit,
        evade,
        followLeader,
        queue,
    }

    #endregion Enums
}