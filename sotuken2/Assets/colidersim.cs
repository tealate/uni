using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colidersim : MonoBehaviour
{
    public List<ContactPoint> colp = new List<ContactPoint>();//�ڐG�ʒu�̏�񃊃X�g
    //public Vector3 sumforce = Vector3.zero; //���ׂẴx�N�g���̍����l(��p�_�͏d�S)
    //public Vector3 sumpos = Vector3.zero;//���ׂĂ̗͂̍��͂̒��͓_
    public (Vector3, Vector3) posfos;//�x�N�g���A���͓_�܂Ƃ߂���i��x��ԁj
    Rigidbody rb;//������rigid�������
    public GameObject tric;//���f������Cube
    public Rigidbody tri; //���f������Cube��rigid
    Vector3 del;//���f���col�L���[�u�Ƃ̍��W��
    public GameObject yubi;//�w(��)


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
    public (Vector3, Vector3) impsim()//�C���p���X��񂩂�v��������
    {
        Vector3 sumforce = Physics.gravity*Time.fixedDeltaTime;//�����l�ŏd�͐����
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
    /*public (Vector3, Vector3) possim()//�ڐG���W����v�Z������
    {

    }*/
    void move(Vector3 force,Vector3 pos)//�������͂̍��́i�x�N�g���A���W�j����ړ��n�_�����߂�
    {
        Vector3 forcepointvec = pos - rb.centerOfMass;

    }
}
