//ä÷êﬂÇÃäpë¨ìxèoóÕ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class velo : MonoBehaviour
{
    public float a;
    public HingeJoint b;
    // Start is called before the first frame update
    void Start()
    {
        b = GetComponent<HingeJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        a = b.velocity;
    }
}
