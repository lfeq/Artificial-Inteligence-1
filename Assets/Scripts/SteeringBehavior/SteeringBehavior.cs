using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior {

    public static Vector3 seek(Agent t_agent, Vector3 t_target) {
        float maxSpeed = t_agent.getMaxSpeed();
        Vector3 currentSpeed = t_agent.getCurrentVelocity();
        float maxForce = t_agent.getMaxForce();
        float mass = t_agent.getMass();
        Vector3 desiredVelocity = t_target - t_agent.transform.position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        desiredVelocity = arrival(t_agent, t_target, desiredVelocity);
        Vector3 steeringForce = desiredVelocity - currentSpeed;
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
        steeringForce /= mass;
        return Vector3.ClampMagnitude((currentSpeed + steeringForce), maxSpeed);
    }

    private static Vector3 arrival(Agent t_agent, Vector3 target, Vector3 desiredVelocity) {
        float distance = Vector3.Distance(t_agent.transform.position, target);
        if (distance <= t_agent.getSlowingRadius()) {
            desiredVelocity.Normalize();
            desiredVelocity *= t_agent.getMaxSpeed() * (distance / t_agent.getSlowingRadius());
        }
        return desiredVelocity;
    }

    public static Vector3 flee(Agent t_agent, Vector3 t_targetPosition) {
        float maxSpeed = t_agent.getMaxSpeed();
        Vector3 currentSpeed = t_agent.getCurrentVelocity();
        float maxForce = t_agent.getMaxForce();
        float mass = t_agent.getMass();
        Vector3 desiredVelocity = t_agent.transform.position - t_targetPosition;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Vector3 steeringForce = desiredVelocity - currentSpeed;
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
        steeringForce /= mass;
        return Vector3.ClampMagnitude((currentSpeed + steeringForce), maxSpeed);
    }

    public static Vector3 pursuit(Agent t_agent, Agent t_agentToPursuit) {
        float distanceToTarget = Vector3.Distance(t_agentToPursuit.transform.position, t_agent.transform.position);
        float positionPrediction = distanceToTarget / t_agent.getMaxSpeed();
        Vector3 futurePosition = t_agentToPursuit.getCurrentVelocity() * positionPrediction;
        futurePosition += t_agentToPursuit.transform.position;
        return seek(t_agent, futurePosition);
    }

    public static Vector3 evade(Agent t_agent, Agent t_agentToPursuit) {
        float distanceToTarget = Vector3.Distance(t_agentToPursuit.transform.position, t_agent.transform.position);
        float positionPrediction = distanceToTarget / t_agent.getMaxSpeed();
        Vector3 futurePosition = t_agentToPursuit.getCurrentVelocity() * positionPrediction;
        futurePosition += t_agentToPursuit.transform.position;
        return flee(t_agent, futurePosition);
    }

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
        return seek(t_agent, target);
    }

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

    public static Vector3 separation(Agent t_agent) {
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
}