using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simu : MonoBehaviour
{
    public Vector3 testforce; //�e�X�g�p�Ɏ��R�ɗ͂�������A������ʒu�͒��S
    public Vector3 tensor;
    public Vector3 contpnt; //�ڐG�ʒu
    public Vector3 masp;  //�d�S
    public Vector3 vecf; //�ڐG�ʒu�̃x�N�g��
    public float force;  //�ڐG�ʒu�̗�
    public Vector3 imp;
    public float impul;
    Rigidbody rb; //�I�u�W�F�N�g���g
    public Vector3 returnforce; //�ڐG�ɑ΂��^�����
    public Vector3 returnbreak; //�ڐG�ɑ΂��^���門�C
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
