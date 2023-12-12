using System.Collections.Generic;
using UnityEngine;

// <summary>
/// The Agent class represents a basic agent.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour {

    #region Public variables

    [HideInInspector] public int currentNodeInPath, pathDirection = 1;
    [HideInInspector] public float wanderAngle = 0.5f;

    #endregion Public variables

    #region Serializable variables

    [SerializeField, Header("Movement")] private float maxSpeed = 5;
    [SerializeField] private float maxForce = 5;
    [SerializeField] private float slowingRadius = 5;
    [SerializeField] private float circleDistance = 5, cirlceRadius = 5;
    [SerializeField] private Transform target;
    [SerializeField, Header("Sight")] private float eyeRadius;
    [SerializeField] private Transform eyePosition;
    [SerializeField, Header("Path Following")] private List<Vector3> nodes = new List<Vector3>();
    [SerializeField] private float pathRadius = 5;

    [SerializeField, Header("Follow Leader")]
    private float leaderBehindDistance = 5, leaderSightRadius = 5,
        separationRadius = 5, maxSeparation = 5;

    [SerializeField] private Agent[] neighbourAgents;
    [SerializeField, Header("Wander")] private float angleChange = 5;
    [SerializeField, Header("Hearing")] private float hearingRadius = 5;
    [SerializeField] private Transform earPositions;
    [SerializeField, Header("Tact")] private float tactRadius = 5;
    [SerializeField] private Transform tactPosition;

    #endregion Serializable variables

    #region private variables

    private Rigidbody rb;

    #endregion private variables

    #region Unity functions

    private void Start() {
        rb = GetComponent<Rigidbody>();
        currentNodeInPath = 0;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(eyePosition.position, eyeRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(earPositions.position, hearingRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(tactPosition.position, tactRadius);
    }

    #endregion Unity functions

    #region Public functions

    public Transform getTargetTranform() {
        return target;
    }

    /// <summary>
    /// Gets the mass of the agent.
    /// </summary>
    /// <returns>The mass of the agent.</returns>
    public float getMass() {
        return rb.mass;
    }

    /// <summary>
    /// Gets the maximum speed of the agent.
    /// </summary>
    /// <returns>The maximum speed of the agent.</returns>
    public float getMaxSpeed() {
        return maxSpeed;
    }

    /// <summary>
    /// Gets the current velocity vector of the agent.
    /// </summary>
    /// <returns>The current velocity vector of the agent.</returns>
    public Vector3 getCurrentVelocity() {
        return rb.velocity;
    }

    /// <summary>
    /// Gets the maximum force that can be applied to the agent.
    /// </summary>
    /// <returns>The maximum force of the agent.</returns>
    public float getMaxForce() {
        return maxForce;
    }

    /// <summary>
    /// Gets the radius within which the agent starts to slow down.
    /// </summary>
    /// <returns>The slowing radius of the agent.</returns>
    public float getSlowingRadius() {
        return slowingRadius;
    }

    /// <summary>
    /// Gets the radius of the agent's visual perception represented by its eyes.
    /// </summary>
    /// <returns>The radius of the agent's visual perception.</returns>
    public float getEyeRadius() {
        return eyeRadius;
    }

    /// <summary>
    /// Gets the position of the agent's eyes.
    /// </summary>
    /// <returns>The position of the agent's eyes.</returns>
    public Vector3 getEyePosition() {
        return eyePosition.position;
    }

    /// <summary>
    /// Gets the distance from the agent to the center of the wandering circle.
    /// </summary>
    /// <returns>The distance from the agent to the wandering circle center.</returns>
    public float getCircleDistance() {
        return circleDistance;
    }

    /// <summary>
    /// Gets the radius of the wandering circle used in the wander behavior.
    /// </summary>
    /// <returns>The radius of the wandering circle.</returns>
    public float getCircleRadius() {
        return cirlceRadius;
    }

    /// <summary>
    /// Gets a list of nodes defining the path for path following behavior.
    /// </summary>
    /// <returns>A list of Transform objects representing path nodes.</returns>
    public List<Vector3> getNodes() {
        if (nodes.Count == 0) {
            nodes = PathFinding.instance.FindPath(transform.position, target.position);
        }
        return nodes;
    }

    public void setNodes(List<Vector3> t_nodeList) {
        nodes.Clear();
        nodes = t_nodeList;
        currentNodeInPath = 0;
    }

    /// <summary>
    /// Gets the radius used for path following behavior.
    /// </summary>
    /// <returns>The radius used for path following behavior.</returns>
    public float getPathRadius() {
        return pathRadius;
    }

    /// <summary>
    /// Gets the distance behind the leader in the follow leader behavior.
    /// </summary>
    /// <returns>The distance behind the leader in the follow leader behavior.</returns>
    public float getLeaderBehindDistance() {
        return leaderBehindDistance;
    }

    /// <summary>
    /// Gets the sight radius used in the follow leader behavior.
    /// </summary>
    /// <returns>The sight radius used in the follow leader behavior.</returns>
    public float getLeaderSightRadius() {
        return leaderSightRadius;
    }

    /// <summary>
    /// Gets an array of neighboring agents used in various behaviors.
    /// </summary>
    /// <returns>An array of neighboring agents.</returns>
    public Agent[] getNeighbourArray() {
        return neighbourAgents;
    }

    /// <summary>
    /// Gets the separation radius used in the separation behavior.
    /// </summary>
    /// <returns>The separation radius used in the separation behavior.</returns>
    public float getSeparationRadius() {
        return separationRadius;
    }

    /// <summary>
    /// Gets the maximum separation distance used in the separation behavior.
    /// </summary>
    /// <returns>The maximum separation distance used in the separation behavior.</returns>
    public float getMaxSeparation() {
        return maxSeparation;
    }

    /// <summary>
    /// Gets the angle change used in the wander behavior.
    /// </summary>
    /// <returns>The angle change used in the wander behavior.</returns>
    public float getAngleChange() {
        return angleChange;
    }

    /// <summary>
    /// Gets the hearing radius used in the hearing behavior.
    /// </summary>
    /// <returns>The hearing radius used in the hearing behavior.</returns>
    public float getHearingRadius() {
        return hearingRadius;
    }

    /// <summary>
    /// Gets the position of the agent's ears.
    /// </summary>
    /// <returns>The position of the agent's ears.</returns>
    public Vector3 getEarsPosition() {
        return earPositions.position;
    }

    /// <summary>
    /// Gets the radius used in the tactile sensing behavior.
    /// </summary>
    /// <returns>The radius used in the tactile sensing behavior.</returns>
    public float getTactRadius() {
        return tactRadius;
    }

    /// <summary>
    /// Gets the position of the tactile sensors.
    /// </summary>
    /// <returns>The position of the tactile sensors.</returns>
    public Vector3 getTactPosition() {
        return tactPosition.position;
    }

    #endregion Public functions
}