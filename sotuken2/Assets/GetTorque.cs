using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTorque : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 torq;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Get the velocity of a wheel, specified by its
    // position in local space.
    void Update()
    {
        torq = rb.GetPointVelocity(transform.TransformPoint(new Vector3(0,0,-0.6f)));
        //rb.AddRelativeTorque(new Vector3(2,2,2));
    }
}
