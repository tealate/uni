using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simu : MonoBehaviour
{
    public Vector3 tensor;
    public Vector3 contpnt; //接触位置
    public Vector3 masp;  //重心
    public Vector3 vecf; //接触位置のベクトル
    public float force;  //接触位置の力
    Rigidbody rb;
    public GameObject obj;
    GameObject ob;
    public int a=0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //GameObject obj = (GameObject)Resources.Load("colid");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(a);
        a = 0;
        tensor = rb.inertiaTensor;
        masp = rb.centerOfMass;
    }
    void OnCollisionStay(Collision collision)
    {
        vecf = collision.impulse;
        contpnt = collision.contacts[0].point;
        foreach (ContactPoint contact in collision.contacts)
        {
            // Visualize the contact point
            Debug.DrawRay(contact.point, contact.normal, Color.black);
            Debug.Log(contact.otherCollider.name);
            a++;
        }
        //for(int i = 0;collision.contacts[i].point != null&& i < 100; i++)
        //Instantiate(obj, collision.contacts[i].point, Quaternion.identity);
        force = collision.impulse.magnitude/Time.deltaTime;
            Debug.DrawRay(transform.TransformPoint(masp), collision.impulse* 3, Color.red,0.0f,false);
    }
}
