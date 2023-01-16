using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//やることリスト:接触判定(検知範囲で処理減らし、頂点座標と面の接触判定)、物理演算(接触座標から合力計算、摩擦の予測計算、forceの取得不可能な物体の物理演算反映)
namespace Mymesh.Unity.Util
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
            //public List<List<Vector3>> SurfaceList;//読み取り用、面のリスト、SurfaceDexListからindexを頂点座標に変えて保持するやつ、一応読み取り用、相互変換実装しろ

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
        public static List<PolObj> ObjList = new List<PolObj>();
        public static List<Col> ColidList = new List<Col>();
        public static List<AllObj> AllObjList = new List<AllObj>();
        public static List<(int, int)> DetList = new List<(int, int)>();

        public static void AddObjList( //リストに各オブジェクトの情報を保存する関数、start()内あたりで呼び出すのがいいはず、知らんけど,awake()か？ ここ書き換えるならObjも書き換えろ
            Transform trans, List<Vector3> TopListl, List<List<int>> SurfaceDexList)
        {
            //ObjList.Add(new PolObj(trans)); ここ直せ
        }

        public static List<(int,int)> Detsim()//検知範囲に触れてる同士のオブジェクトを探す
        {
            float Detsum = 0.0f;
            float possub = 0.0f;
            DetList.Clear();
            for (int i = 0; i < ObjList.Count;i++) //検知範囲に入ったか
            {
                for(int j = i; j < ObjList.Count; j++)
                {
                    Detsum = ObjList[i].Detrange + ObjList[j].Detrange; //検知範囲合算
                    possub = (ObjList[i].Trans.position - ObjList[j].Trans.position).magnitude;//お互いの中心座標からの距離
                    if (Detsum < possub) DetList.Add((i, j));
                }
            }
            return DetList;
        }
    }
}
