using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSomething : MonoBehaviour
{
    //��قǍ쐬�����N���X
    public SerialHandler serialHandler;

    void Start()
    {
        //�M������M�����Ƃ��ɁA���̃��b�Z�[�W�̏������s��
        serialHandler.OnDataReceived += OnDataReceived;
    }

    void Update()
    {
        //������𑗐M
        serialHandler.Write("hogehoge");
    }

    //��M�����M��(message)�ɑ΂��鏈��
    void OnDataReceived(string message)
    {
        string[] data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);
        Debug.Log(data[0]);
        Debug.Log(data[1]);
        if (data.Length < 2) return;

        try
        {
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}
