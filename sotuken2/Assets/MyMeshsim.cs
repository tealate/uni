using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��邱�ƃ��X�g:�ڐG����(���m�͈͂ŏ������炵�A���_���W�Ɩʂ̐ڐG����)�A�������Z(�ڐG���W���獇�͌v�Z�A���C�̗\���v�Z�Aforce�̎擾�s�\�ȕ��̂̕������Z���f)
namespace Mymesh.Unity.Util
{
    /// <summary>
    /// Obj�������Z
    /// </summary>
    public static class Mymesh
    {
        public enum Objtype{Spher,Polygon}//�I�u�W�F�N�g�̌`�A��ꂽ�瑝�₹


        public class PolObj //���p�`�I�u�W�F�N�g�̏�����邽�߂̂�Btrans�͂܂�܁@��������������Ȃ�AddObjList������������
        {
            public Transform Trans;
            public float Detrange = 0f;//�ǂݎ��p�A���m�͈́A��Ԓ��S���狗���̉������_���W�̋���������A���e�l�͕ʂŗp�ӂ��Ă邪�ʂŋ��e�l��ς���Ȃ�R�C�c�ɉ��Z
            public List<Vector3> TopList;//���_���W�̃��X�g�Ascale(1,1,1)�̎��̂��
            public List<List<int>> SurfaceDexList;//�ʂ̃��X�g�A�ʂ��\�����Ă��钸�_���W(TopList)��index�����X�g�œ����
            public List<List<int>> TopDexList;//�ǂݎ��p�A���ꂼ��̒��_�ō\������Ă���ʂ�index���X�g,���_����ʂ̃T�[�`�p
            //public List<List<Vector3>> SurfaceList;//�ǂݎ��p�A�ʂ̃��X�g�ASurfaceDexList����index�𒸓_���W�ɕς��ĕێ������A�ꉞ�ǂݎ��p�A���ݕϊ���������

            public PolObj(Transform Trans,  List<Vector3> TopList,  List<List<int>> SurfaceDexList)
            {
                this.Trans = Trans;
                this.TopList = TopList;
                this.SurfaceDexList = SurfaceDexList;
                foreach(Vector3 Top in TopList)
                {
                    if(this.Detrange < (Top-Trans.position).magnitude)this.Detrange = (Top-Trans.position).magnitude;
                }
                this.Detrange += Tolerance;
                //AllObjList.Add(new AllObj(Trans,this.Detrange,Objtype.Polygon,))
            }
        }

        public class AllObj
        {
            public Transform Trans;
            public float Detrange;
            public Objtype ObjType;
            public int Dex;
            public AllObj(Transform Trans,  float Detrange,  Objtype ObjType,  int Dex)
            {
                this.Trans = Trans;
                this.Detrange = Detrange;
                this.ObjType = ObjType;
                this.Dex = Dex;
            }
        }

        public class Col //�ڐG�_�ێ��p�Bpoint�͐ڐG���W�Anormal�͐ڐG�@��
        {
            public Vector3 point;
            public Vector3 normal;
            public Col(Vector3 point,Vector3 normal)
            {
                this.point = point;
                this.normal = normal;
            }
        }

        public static float Tolerance = 0.02f;//���e�l�A���͈͓̔��Ȃ�ڐG�Ƃ݂Ȃ����
        public static List<PolObj> ObjList = new List<PolObj>();
        public static List<Col> ColidList = new List<Col>();
        public static List<AllObj> AllObjList = new List<AllObj>();
        public static List<(int, int)> DetList = new List<(int, int)>();

        public static void AddObjList( //���X�g�Ɋe�I�u�W�F�N�g�̏���ۑ�����֐��Astart()��������ŌĂяo���̂������͂��A�m��񂯂�,awake()���H ��������������Ȃ�Obj������������
            Transform trans, List<Vector3> TopListl, List<List<int>> SurfaceDexList)
        {
            //ObjList.Add(new PolObj(trans)); ��������
        }

        public static List<(int,int)> Detsim()//���m�͈͂ɐG��Ă铯�m�̃I�u�W�F�N�g��T��
        {
            float Detsum = 0.0f;
            float possub = 0.0f;
            DetList.Clear();
            for (int i = 0; i < ObjList.Count;i++) //���m�͈͂ɓ�������
            {
                for(int j = i; j < ObjList.Count; j++)
                {
                    Detsum = ObjList[i].Detrange + ObjList[j].Detrange; //���m�͈͍��Z
                    possub = (ObjList[i].Trans.position - ObjList[j].Trans.position).magnitude;//���݂��̒��S���W����̋���
                    if (Detsum < possub) DetList.Add((i, j));
                }
            }
            return DetList;
        }
    }
}
