using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour {
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float maxForce = 5;
    [SerializeField] private float slowingRadius = 5;
    [SerializeField] private Transform target;

    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        rb.velocity = SteeringBehavior.seek(this, target);
    }

    public float getMass() {
        return rb.mass;
    }

    public float getMaxSpeed() {
        return maxSpeed;
    }

    public Vector3 getCurrentSpeed() {
        return rb.velocity;
    }

    public float getMaxForce() {
        return maxForce;
    }

    public float getSlowingRadius() {
        return slowingRadius;
    }
}