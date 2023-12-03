using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The SteeringBehaviours class provides functions for implementing steering behaviors in autonomous agents.
/// </summary>
public class SteeringBehaviours {

    #region public functions

    /// <summary>
    /// Implements the "seek" behavior to move the agent towards a specified target.
    /// </summary>
    /// <param name="t_agent">The agent seeking the target.</param>
    /// <param name="t_target">The position of the target.</param>
    /// <param name="useArrival">Flag indicating whether to use arrival behavior.</param>
    /// <returns>The steering force vector for the seek behavior.</returns>
    public static Vector3 seek(Agent t_agent, Vector3 t_target, bool useArrival = false) {
        Vector3 desiredVelocity = t_target - t_agent.transform.position;
        desiredVelocity.Normalize();
        desiredVelocity *= t_agent.getMaxSpeed();
        if (useArrival) {
            desiredVelocity = arrival(t_agent, t_target, desiredVelocity);
        }
        return calculateSteeringForce(t_agent, desiredVelocity);
    }

    /// <summary>
    /// Implements the "flee" behavior to move the agent away from a specified target position.
    /// </summary>
    /// <param name="t_agent">The agent fleeing from the target position.</param>
    /// <param name="t_targetPosition">The position of the target to flee from.</param>
    /// <returns>The steering force vector for the flee behavior.</returns>
    public static Vector3 flee(Agent t_agent, Vector3 t_targetPosition) {
        Vector3 desiredVelocity = t_agent.transform.position - t_targetPosition;
        desiredVelocity.Normalize();
        desiredVelocity *= t_agent.getMaxSpeed();
        return calculateSteeringForce(t_agent, desiredVelocity);
    }

    /// <summary>
    /// Implements the "pursuit" behavior to make the agent pursue a target agent's predicted future position.
    /// </summary>
    /// <param name="t_agent">The agent initiating the pursuit.</param>
    /// <param name="t_agentToPursuit">The target agent to pursue.</param>
    /// <returns>The steering force vector for the pursuit behavior.</returns>
    public static Vector3 pursuit(Agent t_agent, Agent t_agentToPursuit) {
        float distanceToTarget = Vector3.Distance(t_agentToPursuit.transform.position, t_agent.transform.position);
        float positionPrediction = distanceToTarget / t_agent.getMaxSpeed();
        Vector3 futurePosition = t_agentToPursuit.getCurrentVelocity() * positionPrediction;
        futurePosition += t_agentToPursuit.transform.position;
        return seek(t_agent, futurePosition);
    }

    /// <summary>
    /// Implements the "evade" behavior to make the agent move away from the predicted future position of a target agent.
    /// </summary>
    /// <param name="t_agent">The agent evading the target.</param>
    /// <param name="t_agentToPursuit">The target agent from which to evade.</param>
    /// <returns>The steering force vector for the evade behavior.</returns>
    public static Vector3 evade(Agent t_agent, Agent t_agentToPursuit) {
        float distanceToTarget = Vector3.Distance(t_agentToPursuit.transform.position, t_agent.transform.position);
        float positionPrediction = distanceToTarget / t_agent.getMaxSpeed();
        Vector3 futurePosition = t_agentToPursuit.getCurrentVelocity() * positionPrediction;
        futurePosition += t_agentToPursuit.transform.position;
        return flee(t_agent, futurePosition);
    }

    /// <summary>
    /// Implements the "path following" behavior to guide the agent along a predefined path of nodes.
    /// </summary>
    /// <param name="t_agent">The agent following the path.</param>
    /// <returns>The steering force vector for the path following behavior.</returns>
    public static Vector3 pathFollowing(Agent t_agent) {
        Vector3 target;
        List<Transform> nodes = t_agent.getNodes();
        target = nodes[t_agent.currentNodeInPath].position;
        if (Vector3.Distance(target, t_agent.transform.position) < t_agent.getPathRadius()) {
            t_agent.currentNodeInPath += t_agent.pathDirection;
            if ((t_agent.currentNodeInPath >= nodes.Count) || (t_agent.currentNodeInPath < 0)) {
                t_agent.pathDirection *= -1;
                t_agent.currentNodeInPath += t_agent.pathDirection;
            }
        }
        return seek(t_agent, target, true);
    }

    /// <summary>
    /// Implements the "follow leader" behavior to make the agent follow a specified leader agent.
    /// </summary>
    /// <param name="t_leader">The leader agent to follow.</param>
    /// <param name="t_agent">The agent following the leader.</param>
    /// <returns>The steering force vector for the follow leader behavior.</returns>
    public static Vector3 followLeader(Agent t_leader, Agent t_agent) {
        Vector3 leaderVelocity = t_leader.getCurrentVelocity();
        Vector3 force = Vector3.zero, behind, ahead;
        leaderVelocity.Normalize();
        leaderVelocity *= t_agent.getLeaderBehindDistance();
        ahead = t_leader.transform.position;
        ahead += t_leader.getCurrentVelocity();
        leaderVelocity *= -1;
        behind = t_leader.transform.position;
        behind += t_leader.getCurrentVelocity();
        if ((Vector3.Distance(ahead, t_agent.transform.position) <= t_agent.getLeaderSightRadius()) ||
            (Vector3.Distance(t_leader.transform.position, t_agent.transform.position) <= t_agent.getLeaderSightRadius())) {
            force += evade(t_agent, t_leader);
        }
        force += seek(t_agent, behind);
        force += separation(t_agent);
        force = Vector3.ClampMagnitude(force, t_agent.getMaxForce());
        force /= t_agent.getMass();
        return Vector3.ClampMagnitude(t_agent.getCurrentVelocity() - force, t_agent.getMaxSpeed());
    }

    /// <summary>
    /// Implements the "queue" behavior to set velocities for a group of agents, forming a queue.
    /// </summary>
    /// <param name="t_agent">The agent managing the queue.</param>
    public static void queue(Agent t_agent) {
        Agent[] neighbours = t_agent.getNeighbourArray();
        for (int i = 0; i < neighbours.Length; i++) {
            Agent tempAgent = neighbours[i];
            Rigidbody rb = tempAgent.GetComponent<Rigidbody>();
            if (i == 0) {
                rb.velocity = seek(tempAgent, t_agent.transform.position);
                continue;
            }
            rb.velocity = seek(tempAgent, neighbours[i - 1].transform.position);
        }
    }

    /// <summary>
    /// Implements the "wander" behavior to create a random, meandering motion for the agent.
    /// </summary>
    /// <param name="t_agent">The agent applying the wander behavior.</param>
    /// <returns>The steering force vector for the wander behavior.</returns>
    public static Vector3 wander(Agent t_agent) {
        Vector3 circleCenter = t_agent.getCurrentVelocity();
        circleCenter.Normalize();
        circleCenter *= t_agent.getCircleDistance();
        Vector3 displacement = new Vector3(0, 0, -1);
        displacement *= t_agent.getCircleRadius();
        float displacementMagnitud = displacement.magnitude;
        float angleChange = t_agent.getAngleChange();
        displacement.x = Mathf.Cos(t_agent.wanderAngle) * displacementMagnitud;
        displacement.y = Mathf.Sin(t_agent.wanderAngle) * displacementMagnitud;
        t_agent.wanderAngle += Random.value * angleChange - angleChange * 0.5f;
        Vector3 wanderForce = circleCenter + displacement;
        wanderForce = Vector3.ClampMagnitude(wanderForce, t_agent.getMaxSpeed());
        wanderForce /= t_agent.getMass();
        return Vector3.ClampMagnitude((t_agent.getCurrentVelocity() + wanderForce), t_agent.getMaxSpeed());
    }

    #endregion public functions

    #region private functions

    /// <summary>
    /// Implements the "arrival" behavior to slow down the agent as it approaches the target.
    /// </summary>
    /// <param name="t_agent">The agent applying the arrival behavior.</param>
    /// <param name="target">The target position.</param>
    /// <param name="desiredVelocity">The desired velocity vector before arrival adjustments.</param>
    /// <returns>The adjusted steering force vector for the arrival behavior.</returns>
    private static Vector3 arrival(Agent t_agent, Vector3 target, Vector3 desiredVelocity) {
        float distance = Vector3.Distance(t_agent.transform.position, target);
        if (distance <= t_agent.getSlowingRadius()) {
            desiredVelocity.Normalize();
            desiredVelocity *= t_agent.getMaxSpeed() * (distance / t_agent.getSlowingRadius());
        }
        return desiredVelocity;
    }

    /// <summary>
    /// Calculates the steering force to be applied to the agent based on the desired velocity.
    /// </summary>
    /// <param name="t_agent">The agent for which the steering force is calculated.</param>
    /// <param name="t_desiredVelocity">The desired velocity vector.</param>
    /// <returns>The steering force vector for the agent.</returns>
    private static Vector3 calculateSteeringForce(Agent t_agent, Vector3 t_desiredVelocity) {
        Vector3 steeringForce = t_desiredVelocity - t_agent.getCurrentVelocity();
        steeringForce = Vector3.ClampMagnitude(steeringForce, t_agent.getMaxForce());
        steeringForce /= t_agent.getMass(); ;
        return Vector3.ClampMagnitude((t_agent.getCurrentVelocity() + steeringForce), t_agent.getMaxSpeed());
    }

    /// <summary>
    /// Implements the "separation" behavior to keep the agent at a safe distance from neighboring agents.
    /// </summary>
    /// <param name="t_agent">The agent applying separation behavior.</param>
    /// <returns>The steering force vector for the separation behavior.</returns>
    private static Vector3 separation(Agent t_agent) {
        Vector3 force = Vector3.zero;
        int neighbourCount = 0;
        Agent[] neighbours = t_agent.getNeighbourArray();
        for (int i = 0; i < neighbours.Length; i++) {
            Agent tempAgent = neighbours[i];
            if (tempAgent != t_agent &&
                Vector3.Distance(tempAgent.transform.position, t_agent.transform.position) <= t_agent.getSeparationRadius()) {
                force.x += tempAgent.transform.position.x - t_agent.transform.position.x;
                force.z += tempAgent.transform.position.z - t_agent.transform.position.z;
                neighbourCount++;
            }
        }
        if (neighbourCount != 0) {
            force.x /= neighbourCount;
            force.z /= neighbourCount;
            force *= -1;
        }
        force.Normalize();
        force *= t_agent.getMaxSeparation();
        return force;
    }

    #endregion private functions
}