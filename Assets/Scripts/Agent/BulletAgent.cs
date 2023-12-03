using UnityEngine;

/// <summary>
/// MeleeAgent class represents a bullet in the game.
/// </summary>
[RequireComponent(typeof(Agent))]
public class BulletAgent : MonoBehaviour {

    #region Private variables

    private Agent agent;
    private Transform target;
    private Rigidbody rb;

    #endregion Private variables

    #region Unity functions

    private void Start() {
        agent = GetComponent<Agent>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rb.velocity = SteeringBehaviours.seek(agent, target.position);
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Sets the target for the agent to pursue or reach.
    /// </summary>
    /// <param name="t_target">The target transform to set.</param>
    public void setTarget(Transform t_target) {
        target = t_target;
    }

    #endregion Public functions
}