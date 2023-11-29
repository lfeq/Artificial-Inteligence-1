using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour {
    [HideInInspector] public int currentNodeInPath, pathDirection = 1;
    [HideInInspector] public float wanderAngle = 0.5f;

    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float maxForce = 5;
    [SerializeField] private float slowingRadius = 5;
    [SerializeField] private float circleDistance = 5, cirlceRadius = 5;
    [SerializeField] private Transform target;
    [SerializeField] private float eyeRadius;
    [SerializeField] private Transform eyePosition;
    [SerializeField] private List<Transform> nodes = new List<Transform>();
    [SerializeField] private float pathRadius = 5;

    [SerializeField]
    private float leaderBehindDistance = 5, leaderSightRadius = 5,
        separationRadius = 5, maxSeparation = 5;

    [SerializeField] private Agent[] neighbourAgents;
    [SerializeField] private float angleChange = 5;
    [SerializeField, Header("Hearing")] private float hearingRadius = 5;
    [SerializeField] private Transform earPositions;
    [SerializeField, Header("Tact")] private float tactRadius = 5;
    [SerializeField] private Transform tactPosition;

    private Rigidbody rb;

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

    public float getMass() {
        return rb.mass;
    }

    public float getMaxSpeed() {
        return maxSpeed;
    }

    public Vector3 getCurrentVelocity() {
        return rb.velocity;
    }

    public float getMaxForce() {
        return maxForce;
    }

    public float getSlowingRadius() {
        return slowingRadius;
    }

    public float getEyeRadius() {
        return eyeRadius;
    }

    public Vector3 getEyePosition() {
        return eyePosition.position;
    }

    public float getCircleDistance() {
        return circleDistance;
    }

    public float getCircleRadius() {
        return cirlceRadius;
    }

    public List<Transform> getNodes() {
        return nodes;
    }

    public float getPathRadius() {
        return pathRadius;
    }

    public float getLeaderBehindDistance() {
        return leaderBehindDistance;
    }

    public float getLeaderSightRadius() {
        return leaderSightRadius;
    }

    public Agent[] getNeighbourArray() {
        return neighbourAgents;
    }

    public float getSeparationRadius() {
        return separationRadius;
    }

    public float getMaxSeparation() {
        return maxSeparation;
    }

    public float getAngleChange() {
        return angleChange;
    }

    public float getHearingRadius() {
        return hearingRadius;
    }

    public Vector3 getEarsPosition() {
        return earPositions.position;
    }

    public float getTactRadius() {
        return tactRadius;
    }

    public Vector3 getTactPosition() {
        return tactPosition.position;
    }
}