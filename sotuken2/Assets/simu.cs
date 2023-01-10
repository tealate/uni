using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simu : MonoBehaviour
{
    public Vector3 testforce; //テスト用に自由に力を加える、加える位置は中心
    public Vector3 tensor;
    public Vector3 contpnt; //接触位置
    public Vector3 masp;  //重心
    public Vector3 vecf; //接触位置のベクトル
    public float force;  //接触位置の力
    public Vector3 imp;
    public float impul;
    Rigidbody rb; //オブジェクト自身
    public Vector3 returnforce; //接触に対し与える力
    public Vector3 returnbreak; //接触に対し与える摩擦
    public int cont=0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(cont);
        cont = 0;
        tensor = rb.inertiaTensor;
        masp = rb.centerOfMass;
    }
    void OnCollisionStay(Collision collision)
    {
        Debug.Log("a");
        vecf = collision.impulse;
        contpnt = collision.contacts[0].point;
        foreach (ContactPoint contact in collision.contacts)
        {
            // Visualize the contact point
            Debug.DrawRay(contact.point, contact.normal, Color.black);
            Debug.DrawRay(contact.point, contact.impulse * 3, Color.red, 0.0f, false);
            //Debug.Log(contact.otherCollider.name);
            cont++;
        }
        force = collision.impulse.magnitude/Time.deltaTime;
            Debug.DrawRay(transform.TransformPoint(masp), collision.impulse* 3, Color.red,0.0f,false);
    }
}
