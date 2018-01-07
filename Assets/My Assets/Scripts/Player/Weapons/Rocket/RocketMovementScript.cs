using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovementScript : MonoBehaviour {

    [Header("Rocket Properties")]
    [SerializeField] float speed = 10.0f;
    [SerializeField] float torque = 5;
    [HideInInspector] public Transform target;

    //Component References
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!target)
            return;

        rb.AddRelativeForce(0.0f, 0.0f, speed);


        //missile turning momentum
        var step = torque * Time.deltaTime;

        var relativePos = target.position - transform.position;
        var targetRotation = Quaternion.LookRotation(relativePos);
        var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

        rb.MoveRotation(rb.rotation * rotation);
    }

    public void SetRocketTarget(Transform _targetToSet)
    {
        target = _targetToSet;
    }
}
