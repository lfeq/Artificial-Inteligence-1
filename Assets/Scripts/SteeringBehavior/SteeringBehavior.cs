using UnityEngine;

public class SteeringBehavior {

    public static Vector3 seek(Agent t_agent, Transform t_target) {
        float maxSpeed = t_agent.getMaxSpeed();
        Vector3 currentSpeed = t_agent.getCurrentSpeed();
        float maxForce = t_agent.getMaxForce();
        float mass = t_agent.getMass();
        Vector3 desiredVelocity = t_target.position - t_agent.transform.position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        desiredVelocity = arrival(t_agent, t_target.position, desiredVelocity);
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
}