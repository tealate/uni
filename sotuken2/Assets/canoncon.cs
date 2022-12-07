using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canoncon : MonoBehaviour
{
    public int ang;
    private int time = 0;
    public GameObject tama;
    public GameObject iti;
    public SerialHandler serialHandler;
    private int kaku;
    public int tamatama;
    // Start is called before the first frame update
    void Start()
    {
        serialHandler.OnDataReceived += OnDataReceived;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, ang, 0);
        if (tamatama == 0 && time == 10)
        {
            Instantiate(tama);
            time = 0;
        }
        else if (time != 10)
        {
            time++;
        }
    }
    void OnDataReceived(string message)
    {
        string[] data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);
        var deta = Array.ConvertAll(data, int.Parse);
        ang = ((deta[0] + 1) / 3) - 80;
        tamatama = deta[1];
    }
}
