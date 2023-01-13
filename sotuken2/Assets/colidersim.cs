using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colidersim : MonoBehaviour
{
    public List<ContactPoint> colp = new List<ContactPoint>();//�ڐG�ʒu�̏�񃊃X�g
    public Vector3 sumforce = Vector3.zero; //���ׂẴx�N�g���̍����l(��p�_�͏d�S)
    public Vector3 sumpos = Vector3.zero;//���ׂĂ̗͂̍��͂̒��͓_
    public float totalForceMagnitude = 0;
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
