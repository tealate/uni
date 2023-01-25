using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colidersim : MonoBehaviour
{
    public List<ContactPoint> colp = new List<ContactPoint>();//接触位置の情報リスト
    //public Vector3 sumforce = Vector3.zero; //すべてのベクトルの合成値(作用点は重心)
    //public Vector3 sumpos = Vector3.zero;//すべての力の合力の着力点
    public (Vector3, Vector3) posfos;//ベクトル、着力点まとめたやつ（二度手間）
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
        //Debug.Log(transform.TransformPoint(new Vector3(1,1,1)));
        //Debug.Log(transform.InverseTransformPoint(new Vector3(10, 10, 10)));
        //tamesiList.Add(new tamesi(3));
        //Debug.Log(yobidasiList[0].b);
    }
    public class tamesi
    {
        int a;
        public tamesi(int a)
        {
            this.a = a;
            yobidasiList.Add(new yobidasi(tamesiList.Count));
        }
    }
    public static List<tamesi> tamesiList = new List<tamesi>();
    public static List<yobidasi> yobidasiList = new List<yobidasi>();
    public class yobidasi
    {
        public int b;
        public yobidasi(int b)
        {
            this.b = b;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        posfos = impsim();
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
    public (Vector3, Vector3) impsim()//インパルス情報から計測するやつ
    {
        Vector3 sumforce = Physics.gravity*Time.fixedDeltaTime;//初期値で重力先入れ
        Vector3 sumpos = rb.centerOfMass * Physics.gravity.magnitude* Time.fixedDeltaTime;
        float totalForceMagnitude = Physics.gravity.magnitude* Time.fixedDeltaTime;
        if (colp.Count != 0)
        {
            foreach (ContactPoint cont in colp)
            {
                sumforce += cont.impulse;
                sumpos += cont.point * cont.impulse.magnitude;
                totalForceMagnitude += cont.impulse.magnitude;
                Debug.DrawRay(cont.point, cont.impulse * 5, Color.red, 0.0f, false);
            }
            sumpos = sumpos / totalForceMagnitude;
            Debug.DrawRay(sumpos, sumforce * 5, Color.green, 0.0f, false);
            tri.AddForceAtPosition(sumforce, sumpos - del, ForceMode.Impulse);
        }
        colp.Clear();
        return (sumforce, sumpos);
    }
    /*public (Vector3, Vector3) possim()//接触座標から計算するやつ
    {

    }*/
    void move(Vector3 force,Vector3 pos)//加えた力の合力（ベクトル、座標）から移動地点を決める
    {
        Vector3 forcepointvec = pos - rb.centerOfMass;

    }
}
