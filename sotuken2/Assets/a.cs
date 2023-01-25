using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySimulator;

public class a : MonoBehaviour
{
    // Start is called before the first frame update
    const float targetVelocity = 2;
    const float power = 0.6f;
    Rigidbody rb;
    int c;
    /*public Vector3 contpnt;
    public GameObject obj;
    GameObject ob;*/
    List<(int, int)> cath = new List<(int, int)>();
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.UpArrow))
            rb.AddForce(new Vector3(0, 1, 0) * ((targetVelocity - rb.velocity.y) * power), ForceMode.Impulse);
        if (Input.GetKey(KeyCode.DownArrow))
            rb.AddForce(new Vector3(0, -1, 0) * ((targetVelocity + rb.velocity.y) * power), ForceMode.Impulse);
        if (Input.GetKey(KeyCode.LeftArrow))
            rb.AddForce(new Vector3(-1, 0, 0) * ((targetVelocity + rb.velocity.x) * power), ForceMode.Impulse);
        if (Input.GetKey(KeyCode.RightArrow))
            rb.AddForce(new Vector3(1, 0, 0) * ((targetVelocity - rb.velocity.x) * power), ForceMode.Impulse);
        cath = Mymesh.Detsim();
        //Mymesh.ColSearchHub();
        if(Mymesh.ColidList.Count != 0)
        {
            foreach(Mymesh.Col col in Mymesh.ColidList)
            {
                //Debug.DrawRay
            }
            UnityEditor.EditorApplication.isPaused = true;
        }
        c += cath.Count;
        //Debug.Log(Mymesh.AllObjList.Count);
        //Debug.Log(c);
        //for (int i = 0; i <= cath.Count; i++) Debug.Log(cath[i]);
    }
    void OnTriggerStay(Collider other)
    {
        Debug.DrawRay(other.ClosestPointOnBounds(this.transform.position), new Vector3(1, 1, 1), Color.red, 0.0f, false);
    }
}
//ŽŽ‚µ‘‚«A‚·‚®Á‚¹
class RigidBodyContactForceDistribution
{
    public List<Vector3> CalculateForceDistribution(Vector3 force, Vector3 forcePoint, List<Vector3> contactPoints, Vector3 centerOfMass)
    {
        List<Vector3> distributedForces = new List<Vector3>();
        Vector3 torque = Vector3.Cross(forcePoint - centerOfMass, force);
        foreach (Vector3 contactPoint in contactPoints)
        {
            Vector3 moment = Vector3.Cross(contactPoint - centerOfMass, torque);
            Vector3 distributedForce = force + moment;
            distributedForces.Add(distributedForce);
        }
        return distributedForces;
    }
}