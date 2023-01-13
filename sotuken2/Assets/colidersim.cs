using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colidersim : MonoBehaviour
{
    public List<ContactPoint> colp = new List<ContactPoint>();//接触位置の情報リスト
    public Vector3 sumforce = Vector3.zero; //すべてのベクトルの合成値(作用点は重心)
    public Vector3 sumpos = Vector3.zero;//すべての力の合力の着力点
    public float totalForceMagnitude = 0;
    Rigidbody rb;//自分のrigid入れるやつ
    public GameObject tric;//反映させるCube
    public Rigidbody tri; //反映させるCubeのrigid
    Vector3 del;//反映先とcolキューブとの座標差
    public GameObject yubi;//指(仮)


    // Start is called before the first frame update
    void Start()
    {
        tri = tric.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        del = this.transform.position - tric.transform.position;
        rb.sleepThreshold = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (colp.Count != 0)
        {
            sumforce = Vector3.zero;
            sumpos = Vector3.zero;
            foreach (ContactPoint cont in colp)
            {
                sumforce += cont.impulse;
                sumpos += cont.point * cont.impulse.magnitude;
                totalForceMagnitude += cont.impulse.magnitude;
                Debug.DrawRay(cont.point, cont.impulse * 5, Color.red, 0.0f, false);
            }
            sumpos = sumpos / totalForceMagnitude;
            Debug.DrawRay(sumpos,sumforce*5,Color.green,0.0f,false);
            tri.AddForceAtPosition(sumforce,sumpos-del,ForceMode.Impulse);
        }
        colp.Clear();
    }
    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            colp.Add(contact);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            colp.Add(contact);
        }
    }
}
