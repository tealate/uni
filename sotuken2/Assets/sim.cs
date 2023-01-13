using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sim : MonoBehaviour
{
    public Vector3 contpnt; //接触位置
    public Vector3 masp;  //重心
    public Vector3 vecf; //接触位置のベクトル
    public float force;  //接触位置の力
    Rigidbody rb;
    public GameObject obj;
    GameObject ob;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        masp = rb.centerOfMass;
    }
    /*void OnTriggerStay(Collider other)
    {
        vecf = collision.impulse;
        contpnt = collision.contacts[0].point;
        foreach (ContactPoint contact in collision.contacts)
        {
            // Visualize the contact point
            //Debug.DrawRay(contact.point, contact.normal, Color.black);
            //Debug.Log(contact.otherCollider.name);
            a++;
        }
        force = collision.impulse.magnitude / Time.deltaTime;
        //Debug.DrawRay(transform.TransformPoint(masp), collision.impulse* 3, Color.red,0.0f,false);
    } */
    void OnTriggerStay(Collider other)
    {
        //UnityEditor.EditorApplication.isPaused = true;
        Debug.DrawRay(other.ClosestPointOnBounds(this.transform.position), new Vector3(1,1,1), Color.red, 0.0f, false);
    }
}
