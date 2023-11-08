using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior {

    public Vector3 seek(Agent t_agent, Vector3 t_tagetPosition) {
        Vector3 desiredVelocity = t_agent.transform.position - t_tagetPosition;
        desiredVelocity = desiredVelocity.normalized * t_agent.getMaxSpeed(); //Multiplicar por max speed
        Vector3 steering = desiredVelocity - t_agent.getCurrentSpeed();
        //steering = Vector3.Lerp(Vector3.zero, t_agent.getMaxForce(), steering);
        steering /= t_agent.getMass();
        return steering;
    }
}