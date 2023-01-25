using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//やることリスト:接触判定(検知範囲で処理減らし、頂点座標と面の接触判定)、物理演算(接触座標から合力計算、摩擦の予測計算、forceの取得不可能な物体の物理演算反映)
namespace MySimulator
{
    /// <summary>
    /// Obj物理演算
    /// </summary>
    public static class Mymesh
    {
        public enum Objtype{Spher,Polygon}//オブジェクトの形、作れたら増やせ


        public class PolObj //多角形オブジェクトの情報入れるためのやつ。transはまんま　ここ書き換えるならAddObjListも書き換えろ
        {
            public Transform Trans;
            public float Detrange = 0f;//読み取り用、検知範囲、一番中心から距離の遠い頂点座標の距離を入れる、許容値は別で用意してるが個別で許容値を変えるならコイツに加算
            public List<Vector3> TopList;//頂点座標のリスト、scale(1,1,1)の時のやつ
            public List<List<int>> SurfaceDexList;//面のリスト、面を構成している頂点座標(TopList)のindexをリストで入れる
            public List<List<int>> TopDexList;//読み取り用、それぞれの頂点で構成されている面のindexリスト,頂点から面のサーチ用
            public List<List<Vector3>> SurfaceList;//読み取り用、面のリスト、SurfaceDexListからindexを頂点座標に変えて保持するやつ、一応読み取り用、相互変換実装しろ
            private List<Vector3> b;

            public PolObj(Transform Trans,  List<Vector3> TopList,  List<List<int>> SurfaceDexList)//ポリゴンObj情報、呼び出し時に同時にAllObjリスト(trans,検知範囲,Objtype,PolObjリストでのindex番号)にも登録
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

        public class Col //接触点保持用。pointは接触座標、normalは接触法線
        {
            public Vector3 point;
            public Vector3 normal;
            public Col(Vector3 point,Vector3 normal)
            {
                this.point = point;
                this.normal = normal;
            }
        }

        public static float Tolerance = 0.02f;//許容値、この範囲内なら接触とみなされる
        public static List<PolObj> PolObjList = new List<PolObj>(); //ポリゴンのみ
        public static List<Col> ColidList = new List<Col>();
        public static List<AllObj> AllObjList = new List<AllObj>();
        public static List<(int, int)> DetList = new List<(int, int)>();//検知範囲に入っているオブジェクト同士のAllObjList内でのindex
        public static List<int> DetTopList = new List<int>();//検知範囲に入っている頂点座標のindexリスト

        public static void AddPolygonObjList( //PolygonリストにPolygonオブジェクトの情報を保存する関数、start()内あたりで呼び出すのがいいはず、知らんけど,awake()か？ ここ書き換えるならObjも書き換えろ
            Transform trans, List<Vector3> TopList, List<List<int>> SurfaceDexList)
        {
            PolObjList.Add(new PolObj(trans,TopList,SurfaceDexList));
        }

        public static List<(int,int)> Detsim()//検知範囲に触れてる同士のオブジェクトを探す
        {
            Vector3 Deta;//bのローカル座標基準でのx,y,z最大値格納
            Vector3 Detb;//ローカル座標の基準、ローカルから見たx,y,zそれぞれの最大を格納
            Vector3 distanse;// = 0.0f;
            Quaternion c;
            float e;
            Vector3 a;
            Vector3 b;
            Vector3 f;
            //float Detsum = 0.0f;
            //Vector3 possub = Vector3.zero;
            DetList.Clear();
            for (int i = 0; i < AllObjList.Count;i++) //検知範囲に入ったか
            {
                for(int j = i+1; j < AllObjList.Count; j++)
                {
                    //possub = AllObjList[i].Trans.position - AllObjList[j].Trans.position;//お互いの中心座標からの距離
                    //a = (AllObjList[i].Trans.rotation)*  (new Quaternion(AllObjList[i].Trans.localScale.x, AllObjList[i].Trans.localScale.y, AllObjList[i].Trans.localScale.z, 0)) * Quaternion.Inverse(AllObjList[i].Trans.rotation);
                    //Debug.DrawRay(AllObjList[i].Trans.position, (Vector3.Scale(AllObjList[i].Detrange * ((AllObjList[i].Trans.InverseTransformPoint(AllObjList[j].Trans.position)).normalized), AllObjList[i].Trans.localScale)), Color.red,0.0f,false);
                    //Debug.Log((Vector3.Scale((AllObjList[i].Detrange * ((AllObjList[i].Trans.InverseTransformPoint(AllObjList[j].Trans.position)).normalized)), AllObjList[i].Trans.localScale)));
                    //b = (AllObjList[j].Trans.rotation)*  (new Quaternion(AllObjList[j].Trans.localScale.x, AllObjList[j].Trans.localScale.y, AllObjList[j].Trans.localScale.z, 0)) * Quaternion.Inverse(AllObjList[j].Trans.rotation);
                    /*c = AllObjList[j].Trans.rotation * new Quaternion(1,0,0,0) * Quaternion.Inverse(AllObjList[j].Trans.rotation);//j基準のx,y,z座標の方向を求めるベクトル一個でやるなxyz三つで分けろ
                    c = Quaternion.Inverse(AllObjList[i].Trans.rotation) * c * AllObjList[i].Trans.rotation;//j基準のx,y,z座標をi基準に戻す
                    f = (new Vector3(c.x,c.y,c.z)).normalized;
                    //Debug.Log(new Vector3(c.x, c.y, c.z));
                    //Debug.DrawRay(AllObjList[i].Trans.position, new Vector3(c.x, c.y, c.z), Color.red, 0.0f, false);
                    a = Vector3.Scale(f, f);
                    b = Vector3.Scale(AllObjList[i].Trans.localScale, AllObjList[i].Trans.localScale);
                    e = Mathf.Sqrt(1/((a.x/b.x)+(a.y/b.y)+(a.z/b.z)));
                    Deta.x = AllObjList[i].Detrange * e;//bローカル座標から見たaのx座標方向最大値

                    c = AllObjList[j].Trans.rotation * new Quaternion(0, 1, 0, 0) * Quaternion.Inverse(AllObjList[j].Trans.rotation);//j基準のx,y,z座標の方向を求めるベクトル一個でやるなxyz三つで分けろ
                    c = Quaternion.Inverse(AllObjList[i].Trans.rotation) * c * AllObjList[i].Trans.rotation;//j基準のx,y,z座標をi基準に戻す
                    f = (new Vector3(c.x, c.y, c.z)).normalized;
                    //Debug.DrawRay(AllObjList[i].Trans.position, new Vector3(c.x, c.y, c.z), Color.red, 0.0f, false);
                    a = Vector3.Scale(f,f);
                    b = Vector3.Scale(AllObjList[i].Trans.localScale, AllObjList[i].Trans.localScale);
                    e = Mathf.Sqrt(1/((a.x / b.x) + (a.y / b.y) + (a.z / b.z)));
                    //Debug.Log(e);
                    Deta.y = AllObjList[i].Detrange * e;//bローカル座標から見たaのx座標方向最大値

                    c = AllObjList[j].Trans.rotation * new Quaternion(0, 0, 1, 0) * Quaternion.Inverse(AllObjList[j].Trans.rotation);//j基準のx,y,z座標の方向を求めるベクトル一個でやるなxyz三つで分けろ
                    c = Quaternion.Inverse(AllObjList[i].Trans.rotation) * c * AllObjList[i].Trans.rotation;//j基準のx,y,z座標をi基準に戻す
                    f = (new Vector3(c.x, c.y, c.z)).normalized;
                    //Debug.DrawRay(AllObjList[i].Trans.position,f,Color.green,0.0f,false);
                    //Debug.Log(f);
                    //Debug.DrawRay(AllObjList[i].Trans.position, new Vector3(c.x, c.y, c.z), Color.red, 0.0f, false);
                    a = Vector3.Scale(f,f);
                    b = Vector3.Scale(AllObjList[i].Trans.localScale, AllObjList[i].Trans.localScale);
                    e = Mathf.Sqrt(1/((a.x / b.x) + (a.y / b.y) + (a.z / b.z)));
                    //Debug.Log("e="+e);
                    Deta.z = AllObjList[i].Detrange * e;//bローカル座標から見たaのz座標方向最大値

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

                    //Debug.Log("正解" + (distanse.magnitude - (AllObjList[j].Trans.position - AllObjList[i].Trans.position).magnitude));
                    //Debug.Log("中心との差"+distanse);
                    //Debug.DrawRay(AllObjList[i].Trans.position, new Vector3(c.x, c.y, c.z), Color.red, 0.0f, false);
                    //Debug.DrawRay(AllObjList[j].Trans.position, (Vector3.Scale(AllObjList[j].Detrange * ((AllObjList[j].Trans.InverseTransformPoint(AllObjList[i].Trans.position)).normalized), AllObjList[j].Trans.localScale)), Color.red, 0.0f, false);
                    //Detsum = Deta + Detb; //検知範囲合算
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

                    //一時的な処置、localscaleから一番遠い頂点の距離＋許容値の円内で検知円を作成。後で楕円作れ
                    if ((AllObjList[i].Trans.localScale.magnitude * AllObjList[i].Detrange) + (AllObjList[j].Trans.localScale.magnitude * AllObjList[j].Detrange) + Tolerance > (AllObjList[i].Trans.position - AllObjList[j].Trans.position).magnitude)
                    {
                        DetList.Add((i, j));
                        Debug.DrawLine(AllObjList[i].Trans.position, AllObjList[j].Trans.position, Color.red, 0.0f, false);
                    }
                }
            }
            return DetList;
        }


        public static List<int> DetTopsim(int Det, int Top)//検知範囲に入っている頂点座標探すやつ。Detに検知円の砲のオブジェクトのindex、AllObjList参照、Topが頂点側のオブジェクトのindex、PolObjList参照
        {
            DetTopList.Clear();
            for(int i = 0; i < PolObjList[Top].TopList.Count; i++)
            {
                if (AllObjList[Det].Trans.localScale.magnitude * AllObjList[Det].Detrange > ((PolObjList[Top].Trans.TransformPoint(PolObjList[Top].TopList[i])) - AllObjList[Det].Trans.position).magnitude) DetTopList.Add(i);
            }
            return DetTopList;
        }


        /*public static void ColSearchHub()//接触検知の中心 オブジェクト形状の組み合わせごとに適切な接触の判定を行う関数呼ぶやつ
        {
            List<(int, int)> DetObj = Detsim();// 検知円触れ合った同士のオブジェリストIndex、AllObjList参照
            foreach((int,int) Obj in DetObj)//オブジェ同士の接触判定、swicthでオブジェaの各頂点、各辺とオブジェbの各面との接触判定、そのあとa,b逆でもう一度
            {
                switch (AllObjList[Obj.Item1].ObjType)
                {
                    case Objtype.Polygon:
                        PolColSerch(AllObjList[Obj.Item1].Dex,Obj.Item2);
                        break;
                    case Objtype.Spher:
                        break;
                    default:
                        Debug.LogError(AllObjList[Obj.Item1].ObjType + "は設定されていません");
                        break;
                }
            }
        }


        public static void PolColSerch(int Poldex,int Objdex)//Polygon各頂点が指定オブジェクトに接触しているか
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
