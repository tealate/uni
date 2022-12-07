// 関節に対してw,Sキーでトルクを与えるスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtorque : MonoBehaviour
{
    public JointMotor motor;
    public HingeJoint hinge;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        hinge.useMotor = true;
        motor = hinge.motor;
        motor.force = 0;
        motor.freeSpin = false;
        motor.targetVelocity = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            motor.force += 1;
        if (Input.GetKey(KeyCode.S))
            motor.force -= 1;
        force = motor.force;
        hinge.motor = motor;
        // if (Input.GetKey(KeyCode.LeftArrow))
        //    this.transform.Translate(0, 0, -0.1f);
        // if (Input.GetKey(KeyCode.RightArrow))
        //    this.transform.Translate(0, 0, 0.1f);
    }
}
