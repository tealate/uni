using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obj;
    public Vector3 a;
    GameObject ob;
    // Update is called once per frame
    void Start()
    {
        a = transform.position;
    }
    void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            a += (new Vector3(0, 0.02f, 0));
        if (Input.GetKey(KeyCode.DownArrow))
            a += (new Vector3(0, -0.02f, 0));
        if (Input.GetKey(KeyCode.LeftArrow))
            a += (new Vector3(-0.02f, 0, 0));
        if (Input.GetKey(KeyCode.RightArrow))
            a += (new Vector3(0.02f, 0, 0));
        transform.position = a;
    }
}

