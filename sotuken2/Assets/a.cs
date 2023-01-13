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
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.UpArrow))
            rb.AddForce(new Vector3(0, 1, 0) * ((targetVelocity - rb.velocity.y) * power), ForceMode.Impulse);
        if (Input.GetKey(KeyCode.DownArrow))
            rb.AddForce(new Vector3(0, -1, 0) * ((targetVelocity + rb.velocity.y) * power), ForceMode.Impulse);
        if (Input.GetKey(KeyCode.LeftArrow))
            rb.AddForce(new Vector3(-1, 0, 0) * ((targetVelocity + rb.velocity.x) * power), ForceMode.Impulse);
        if (Input.GetKey(KeyCode.RightArrow))
            rb.AddForce(new Vector3(1, 0, 0) * ((targetVelocity - rb.velocity.x) * power), ForceMode.Impulse);
    }
    void OnTriggerStay(Collider other)
    {
        Debug.DrawRay(other.ClosestPointOnBounds(this.transform.position), new Vector3(1, 1, 1), Color.red, 0.0f, false);
    }
}
