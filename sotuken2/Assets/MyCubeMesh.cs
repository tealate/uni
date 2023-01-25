using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySimulator;

public class MyCubeMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mymesh.AddPolygonObjList(GetComponent<Transform>(),
            //���_���X�g��������
            new List<Vector3>(){
               new Vector3(0.5f, 0.5f, 0.5f),
               new Vector3(0.5f, 0.5f, -0.5f),
               new Vector3(0.5f, -0.5f, 0.5f),
               new Vector3(0.5f, -0.5f, -0.5f),
               new Vector3(-0.5f, 0.5f, 0.5f),
               new Vector3(-0.5f, 0.5f, -0.5f),
               new Vector3(-0.5f, -0.5f, 0.5f),
               new Vector3(-0.5f, -0.5f, -0.5f)},
            //���_���X�g�����܂�

            //�ʃ��X�g��������
            new List<List<int>>()
            {
                new List<int>(){0,1,2,3},
                new List<int>(){4,5,6,7},
                new List<int>(){0,1,4,5},
                new List<int>(){2,3,6,7},
                new List<int>(){0,2,4,6},
                new List<int>(){1,3,5,7}
            }
            //�ʃ��X�g�����܂�
            );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
