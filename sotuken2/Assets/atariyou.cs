using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySimulator;

public class atariyou : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(Mymesh.DetList.Count);
    }
    void OnTriggerStay(Collider other) 
    {
        Debug.Log(Mymesh.DetList.Count);
    }
}
