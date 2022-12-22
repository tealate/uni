using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a : MonoBehaviour
{
    // Start is called before the first frame update
    const float targetVelocity = 2;
    const float power = 0.6f;
    Rigidbody rb;
    /*public Vector3 contpnt;
    public GameObject obj;
    GameObject ob;*/
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
            rb.AddForce(new Vector3(0, 1, 0) * ((targetVelocity - rb.velocity.y) * power), ForceMode.Acceleration);
        if (Input.GetKey(KeyCode.DownArrow))
            rb.AddForce(new Vector3(0, -1, 0) * ((targetVelocity + rb.velocity.y) * power), ForceMode.Acceleration);
        if (Input.GetKey(KeyCode.LeftArrow))
            rb.AddForce(new Vector3(-1, 0, 0) * ((targetVelocity + rb.velocity.x) * power), ForceMode.Acceleration);
        if (Input.GetKey(KeyCode.RightArrow))
            rb.AddForce(new Vector3(1, 0, 0) * ((targetVelocity - rb.velocity.x) * power), ForceMode.Acceleration);
    }
    /*void OnCollisionEnter(Collision collision)
    {
        Destroy(ob);
        contpnt = collision.contacts[0].point;
        ob = (GameObject)Instantiate(obj, collision.contacts[0].point, Quaternion.identity);
    }*/
}
