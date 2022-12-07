using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class testangle : MonoBehaviour
{
    private string a;
    private bool b = true;
    public int c;
    public int d;
    public SerialHandler serialHandler;
    public JointSpring sp;
    public HingeJoint hinge;
    public float torque;
    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        sp = hinge.spring;
        sp.spring = 10;
        sp.damper = 3;
        sp.targetPosition = 0;
        hinge.spring = sp;
        hinge.useSpring = true;
        serialHandler.OnDataReceived += OnDataReceived;
    }

    // Update is called once per frame
    void Update()
    {
        if (b && Input.GetKey(KeyCode.X))
        {
            b = false;
            StreamWriter sw = new StreamWriter("../TextData.txt", false);
            sw.WriteLine(a);
            sw.Flush();
            sw.Close();
        }
    }
    void OnDataReceived(string message)
    {
        string[] data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);
        var deta = Array.ConvertAll(data, int.Parse);
        //ang = ((deta[0] + 1) / 3) - 80;
        c = deta[0];
        d = deta[1];
        a += data[0] + ",";
        a += data[1] + ",";
        sp.targetPosition = (deta[0] - 872)/4.64f;
        torque = (deta[0] - 872)/4.64f;
        a += torque + "\n";
        hinge.spring = sp;
    }
}
