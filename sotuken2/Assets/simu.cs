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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //GameObject obj = (GameObject)Resources.Load("colid");
    }

    // Update is called once per frame
    void Update()
    {
        tensor = rb.inertiaTensor;
        masp = rb.centerOfMass;
    }
    void OnCollisionStay(Collision collision)
    {
        Destroy(ob);
        vecf = collision.impulse;
        contpnt = collision.contacts[0].point;
        ob = (GameObject)Instantiate(obj, collision.contacts[0].point, Quaternion.identity);
        force = collision.impulse.magnitude/Time.deltaTime;
        Debug.DrawRay(contpnt,vecf*3, Color.red,0.0f,false);
    }
}
