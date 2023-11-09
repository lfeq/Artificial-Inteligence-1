using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour {
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float maxForce = 5;

    private Rigidbody rb;
    private SteeringBehavior steeringBehavior;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        steeringBehavior = new SteeringBehavior();
    }

    private void Update() {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        print(target);
        rb.velocity += steeringBehavior.seek(this, target);
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
}