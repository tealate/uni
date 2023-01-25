using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��邱�ƃ��X�g:�ڐG����(���m�͈͂ŏ������炵�A���_���W�Ɩʂ̐ڐG����)�A�������Z(�ڐG���W���獇�͌v�Z�A���C�̗\���v�Z�Aforce�̎擾�s�\�ȕ��̂̕������Z���f)
namespace MySimulator
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
            public List<List<Vector3>> SurfaceList;//�ǂݎ��p�A�ʂ̃��X�g�ASurfaceDexList����index�𒸓_���W�ɕς��ĕێ������A�ꉞ�ǂݎ��p�A���ݕϊ���������
            private List<Vector3> b;

            public PolObj(Transform Trans,  List<Vector3> TopList,  List<List<int>> SurfaceDexList)//�|���S��Obj���A�Ăяo�����ɓ�����AllObj���X�g(trans,���m�͈�,Objtype,PolObj���X�g�ł�index�ԍ�)�ɂ��o�^
            {
                this.Trans = Trans;
                this.TopList = TopList;
                this.SurfaceDexList = SurfaceDexList;
                /*foreach(List<int> dex in SurfaceDexList)
                {
                    foreach(int top in dex)
                    {
                        this.b.Add(this.TopList[top]);
                    }
                    SurfaceList.Add(this.b);
                    this.b.Clear();
                }*/
                foreach(Vector3 Top in TopList)
                {
                    if(this.Detrange < Top.magnitude)this.Detrange = Top.magnitude;
                }
                this.Detrange += Tolerance;
                AllObjList.Add(new AllObj(Trans, this.Detrange, Objtype.Polygon, PolObjList.Count-1));
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
        public static List<PolObj> PolObjList = new List<PolObj>(); //�|���S���̂�
        public static List<Col> ColidList = new List<Col>();
        public static List<AllObj> AllObjList = new List<AllObj>();
        public static List<(int, int)> DetList = new List<(int, int)>();//���m�͈͂ɓ����Ă���I�u�W�F�N�g���m��AllObjList���ł�index
        public static List<int> DetTopList = new List<int>();//���m�͈͂ɓ����Ă��钸�_���W��index���X�g

        public static void AddPolygonObjList( //Polygon���X�g��Polygon�I�u�W�F�N�g�̏���ۑ�����֐��Astart()��������ŌĂяo���̂������͂��A�m��񂯂�,awake()���H ��������������Ȃ�Obj������������
            Transform trans, List<Vector3> TopList, List<List<int>> SurfaceDexList)
        {
            PolObjList.Add(new PolObj(trans,TopList,SurfaceDexList));
        }

        public static List<(int,int)> Detsim()//���m�͈͂ɐG��Ă铯�m�̃I�u�W�F�N�g��T��
        {
            Vector3 Deta;//b�̃��[�J�����W��ł�x,y,z�ő�l�i�[
            Vector3 Detb;//���[�J�����W�̊�A���[�J�����猩��x,y,z���ꂼ��̍ő���i�[
            Vector3 distanse;// = 0.0f;
            Quaternion c;
            float e;
            Vector3 a;
            Vector3 b;
            Vector3 f;
            //float Detsum = 0.0f;
            //Vector3 possub = Vector3.zero;
            DetList.Clear();
            for (int i = 0; i < AllObjList.Count;i++) //���m�͈͂ɓ�������
            {
                for(int j = i+1; j < AllObjList.Count; j++)
                {
                    //possub = AllObjList[i].Trans.position - AllObjList[j].Trans.position;//���݂��̒��S���W����̋���
                    //a = (AllObjList[i].Trans.rotation)*  (new Quaternion(AllObjList[i].Trans.localScale.x, AllObjList[i].Trans.localScale.y, AllObjList[i].Trans.localScale.z, 0)) * Quaternion.Inverse(AllObjList[i].Trans.rotation);
                    //Debug.DrawRay(AllObjList[i].Trans.position, (Vector3.Scale(AllObjList[i].Detrange * ((AllObjList[i].Trans.InverseTransformPoint(AllObjList[j].Trans.position)).normalized), AllObjList[i].Trans.localScale)), Color.red,0.0f,false);
                    //Debug.Log((Vector3.Scale((AllObjList[i].Detrange * ((AllObjList[i].Trans.InverseTransformPoint(AllObjList[j].Trans.position)).normalized)), AllObjList[i].Trans.localScale)));
                    //b = (AllObjList[j].Trans.rotation)*  (new Quaternion(AllObjList[j].Trans.localScale.x, AllObjList[j].Trans.localScale.y, AllObjList[j].Trans.localScale.z, 0)) * Quaternion.Inverse(AllObjList[j].Trans.rotation);
                    /*c = AllObjList[j].Trans.rotation * new Quaternion(1,0,0,0) * Quaternion.Inverse(AllObjList[j].Trans.rotation);//j���x,y,z���W�̕��������߂�x�N�g����ł���xyz�O�ŕ�����
                    c = Quaternion.Inverse(AllObjList[i].Trans.rotation) * c * AllObjList[i].Trans.rotation;//j���x,y,z���W��i��ɖ߂�
                    f = (new Vector3(c.x,c.y,c.z)).normalized;
                    //Debug.Log(new Vector3(c.x, c.y, c.z));
                    //Debug.DrawRay(AllObjList[i].Trans.position, new Vector3(c.x, c.y, c.z), Color.red, 0.0f, false);
                    a = Vector3.Scale(f, f);
                    b = Vector3.Scale(AllObjList[i].Trans.localScale, AllObjList[i].Trans.localScale);
                    e = Mathf.Sqrt(1/((a.x/b.x)+(a.y/b.y)+(a.z/b.z)));
                    Deta.x = AllObjList[i].Detrange * e;//b���[�J�����W���猩��a��x���W�����ő�l

                    c = AllObjList[j].Trans.rotation * new Quaternion(0, 1, 0, 0) * Quaternion.Inverse(AllObjList[j].Trans.rotation);//j���x,y,z���W�̕��������߂�x�N�g����ł���xyz�O�ŕ�����
                    c = Quaternion.Inverse(AllObjList[i].Trans.rotation) * c * AllObjList[i].Trans.rotation;//j���x,y,z���W��i��ɖ߂�
                    f = (new Vector3(c.x, c.y, c.z)).normalized;
                    //Debug.DrawRay(AllObjList[i].Trans.position, new Vector3(c.x, c.y, c.z), Color.red, 0.0f, false);
                    a = Vector3.Scale(f,f);
                    b = Vector3.Scale(AllObjList[i].Trans.localScale, AllObjList[i].Trans.localScale);
                    e = Mathf.Sqrt(1/((a.x / b.x) + (a.y / b.y) + (a.z / b.z)));
                    //Debug.Log(e);
                    Deta.y = AllObjList[i].Detrange * e;//b���[�J�����W���猩��a��x���W�����ő�l

                    c = AllObjList[j].Trans.rotation * new Quaternion(0, 0, 1, 0) * Quaternion.Inverse(AllObjList[j].Trans.rotation);//j���x,y,z���W�̕��������߂�x�N�g����ł���xyz�O�ŕ�����
                    c = Quaternion.Inverse(AllObjList[i].Trans.rotation) * c * AllObjList[i].Trans.rotation;//j���x,y,z���W��i��ɖ߂�
                    f = (new Vector3(c.x, c.y, c.z)).normalized;
                    //Debug.DrawRay(AllObjList[i].Trans.position,f,Color.green,0.0f,false);
                    //Debug.Log(f);
                    //Debug.DrawRay(AllObjList[i].Trans.position, new Vector3(c.x, c.y, c.z), Color.red, 0.0f, false);
                    a = Vector3.Scale(f,f);
                    b = Vector3.Scale(AllObjList[i].Trans.localScale, AllObjList[i].Trans.localScale);
                    e = Mathf.Sqrt(1/((a.x / b.x) + (a.y / b.y) + (a.z / b.z)));
                    //Debug.Log("e="+e);
                    Deta.z = AllObjList[i].Detrange * e;//b���[�J�����W���猩��a��z���W�����ő�l

                    //Debug.Log(Deta);
                    //Debug.Log(-1*Deta);
                    Detb = AllObjList[j].Detrange * AllObjList[j].Trans.localScale;
                    //Debug.Log(Detb);
                    //Debug.Log(Deta);
                    //Debug.Log(Deta + Detb);
                    //Debug.DrawRay(AllObjList[j].Trans.position, Detb, Color.red, 0.0f, false);
                    //Debug.DrawRay(AllObjList[j].Trans.position, -1 * Detb, Color.blue, 0.0f, false);
                    //Debug.Log(Detb);
                    distanse = Vector3.Scale(AllObjList[j].Trans.localScale , AllObjList[j].Trans.InverseTransformPoint(AllObjList[i].Trans.position));// new Vector3(b.x, b.y, b.z)* AllObjList[j].Detrange;
                    //distanse = AllObjList[j].Trans.TransformDirection(AllObjList[j].Trans.position - AllObjList[i].Trans.position);*/
                    //distanse = AllObjList[j].Trans.position - AllObjList[i].Trans.position;
                    //Deta = Vector3.Scale(AllObjList[i].Trans.localScale, AllObjList[i].Trans.InverseTransformDirection(distanse.normalized));
                    //Detb = Vector3.Scale(AllObjList[j].Trans.localScale, AllObjList[j].Trans.InverseTransformDirection(-1 * distanse.normalized));
                    //Deta = AllObjList[i].Detrange * AllObjList[i].Trans.TransformDirection(Deta);
                    //Detb = AllObjList[j].Detrange * AllObjList[j].Trans.TransformDirection(Detb);
                    //Debug.DrawRay(AllObjList[i].Trans.position, Deta, Color.red, 0.0f, false);
                    //Debug.DrawRay(AllObjList[j].Trans.position, Detb, Color.red, 0.0f, false);

                    //Debug.Log("����" + (distanse.magnitude - (AllObjList[j].Trans.position - AllObjList[i].Trans.position).magnitude));
                    //Debug.Log("���S�Ƃ̍�"+distanse);
                    //Debug.DrawRay(AllObjList[i].Trans.position, new Vector3(c.x, c.y, c.z), Color.red, 0.0f, false);
                    //Debug.DrawRay(AllObjList[j].Trans.position, (Vector3.Scale(AllObjList[j].Detrange * ((AllObjList[j].Trans.InverseTransformPoint(AllObjList[i].Trans.position)).normalized), AllObjList[j].Trans.localScale)), Color.red, 0.0f, false);
                    //Detsum = Deta + Detb; //���m�͈͍��Z
                    //if (Detsum < possub.magnitude) DetList.Add((i, j));
                    //Debug.DrawRay(AllObjList[i].Trans.position, AllObjList[j].Trans.InverseTransformPoint(Vector3.Scale(Deta, new Vector3(1, 0, 0))), Color.red, 0.0f, false);
                    //Debug.DrawRay(AllObjList[i].Trans.position, AllObjList[j].Trans.InverseTransformPoint(Vector3.Scale(Deta, new Vector3(0, 1, 0))), Color.red, 0.0f, false);
                    //Debug.DrawRay(AllObjList[i].Trans.position, AllObjList[j].Trans.InverseTransformPoint(Vector3.Scale(Deta, new Vector3(0, 0, 1))), Color.red, 0.0f, false);
                    //Debug.DrawRay(AllObjList[j].Trans.position, Vector3.Scale(Detb, new Vector3(1, 0, 0)), Color.red, 0.0f, false);
                    //Debug.DrawRay(AllObjList[j].Trans.position, Vector3.Scale(Detb, new Vector3(0, 1, 0)), Color.red, 0.0f, false);
                    //Debug.DrawRay(AllObjList[j].Trans.position, Vector3.Scale(Detb, new Vector3(0, 0, 1)), Color.red, 0.0f, false);
                    //if (Tamesi(AllObjList[i].Trans.position, AllObjList[i].Trans.rotation, Deta, AllObjList[j].Trans.position, AllObjList[j].Trans.rotation,Detb)) DetList.Add((i, j));
                    //Debug.Log(AllObjList[i].Detrange * AllObjList[i].Trans.localScale);
                    //if (distanse.magnitude < Deta.magnitude + Detb.magnitude) DetList.Add((i, j));
                    //if (Mathf.Abs(distanse.x) < Mathf.Abs(Deta.x) + Mathf.Abs(Detb.x) && Mathf.Abs(distanse.y) < Mathf.Abs(Deta.y) + Mathf.Abs(Detb.y) && Mathf.Abs(distanse.z) < Mathf.Abs(Deta.z) + Mathf.Abs(Detb.z)) DetList.Add((i, j));

                    //�ꎞ�I�ȏ��u�Alocalscale�����ԉ������_�̋����{���e�l�̉~���Ō��m�~���쐬�B��őȉ~���
                    if ((AllObjList[i].Trans.localScale.magnitude * AllObjList[i].Detrange) + (AllObjList[j].Trans.localScale.magnitude * AllObjList[j].Detrange) + Tolerance > (AllObjList[i].Trans.position - AllObjList[j].Trans.position).magnitude)
                    {
                        DetList.Add((i, j));
                        Debug.DrawLine(AllObjList[i].Trans.position, AllObjList[j].Trans.position, Color.red, 0.0f, false);
                    }
                }
            }
            return DetList;
        }


        public static List<int> DetTopsim(int Det, int Top)//���m�͈͂ɓ����Ă��钸�_���W�T����BDet�Ɍ��m�~�̖C�̃I�u�W�F�N�g��index�AAllObjList�Q�ƁATop�����_���̃I�u�W�F�N�g��index�APolObjList�Q��
        {
            DetTopList.Clear();
            for(int i = 0; i < PolObjList[Top].TopList.Count; i++)
            {
                if (AllObjList[Det].Trans.localScale.magnitude * AllObjList[Det].Detrange > ((PolObjList[Top].Trans.TransformPoint(PolObjList[Top].TopList[i])) - AllObjList[Det].Trans.position).magnitude) DetTopList.Add(i);
            }
            return DetTopList;
        }


        /*public static void ColSearchHub()//�ڐG���m�̒��S �I�u�W�F�N�g�`��̑g�ݍ��킹���ƂɓK�؂ȐڐG�̔�����s���֐��ĂԂ��
        {
            List<(int, int)> DetObj = Detsim();// ���m�~�G�ꍇ�������m�̃I�u�W�F���X�gIndex�AAllObjList�Q��
            foreach((int,int) Obj in DetObj)//�I�u�W�F���m�̐ڐG����Aswicth�ŃI�u�W�Fa�̊e���_�A�e�ӂƃI�u�W�Fb�̊e�ʂƂ̐ڐG����A���̂���a,b�t�ł�����x
            {
                switch (AllObjList[Obj.Item1].ObjType)
                {
                    case Objtype.Polygon:
                        PolColSerch(AllObjList[Obj.Item1].Dex,Obj.Item2);
                        break;
                    case Objtype.Spher:
                        break;
                    default:
                        Debug.LogError(AllObjList[Obj.Item1].ObjType + "�͐ݒ肳��Ă��܂���");
                        break;
                }
            }
        }


        public static void PolColSerch(int Poldex,int Objdex)//Polygon�e���_���w��I�u�W�F�N�g�ɐڐG���Ă��邩
        {
            switch (AllObjList[Objdex].ObjType)
            {
                case Objtype.Polygon:
                    Vector3 PolScale = PolObjList[Poldex].Trans.localScale;
                    Vector3 ObjScale = AllObjList[Objdex].Trans.localScale;
                    List<Vector3> PolTopList = PolObjList[Poldex].TopList;
                    List<Vector3> ObjTopList = PolObjList[AllObjList[Objdex].Dex].TopList;
                    foreach (Vector3 Top in PolTopList)
                    {
                        foreach(List<Vector3> surface in PolObjList[AllObjList[Objdex].Dex].SurfaceList)
                        {
                            bool Contf = true;
                            Vector3 normal = Vector3.zero;
                            for (int i = 0; i < ObjTopList.Count - 2; i++)
                            {
                                normal = Vector3.Cross(ObjTopList[i + 1] - ObjTopList[i], ObjTopList[i + 2] - ObjTopList[i]).normalized;
                                float distance = Vector3.Dot(normal, ObjTopList[i] - Top);
                                if (distance > Tolerance)
                                {
                                    Contf = false;
                                    break;
                                }
                            }
                            if (Contf)
                            {
                                ColidList.Add(new Col(Top, normal));
                            }
                        }
                    }
                    break;
            }
        }*/
    }
}
