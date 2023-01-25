using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    public Vector3 Pos;
    public GameObject a;
    Transform b;
    // Start is called before the first frame update
    void Start()
    {
        b = a.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = b.position + Pos;
    }
}
