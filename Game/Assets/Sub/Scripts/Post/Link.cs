using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    public class Link : MonoBehaviour
    {
        //接続するオブジェクト
        [SerializeField]
        private GameObject[] linkObj_;
        public GameObject[] LinkObject
        {
            get
            {
                return linkObj_;
            }
            set
            {
                linkObj_ = value;
            }
           
        }
        //接続しているオブジェクトの距離
        [SerializeField]
        private List<float> linkLength_;
        public List<float> LinkLength
        {
            get
            {
                return linkLength_;
            }
        }
        //ID
        [SerializeField]
        private int linkID_;
        public int LinkID
        {
            get
            {
                return linkID_;
            }
        }
        // Start is called before the first frame update
        void Awake()
        {
            InitLink();
        }

        // Update is called once per frame
        void Update()
        {

        }

        //リンクの初期化
        public void InitLink()
        {
            //接続しているオブジェクトの距離をリストに入れる
            linkLength_ = new List<float>(linkLength_.Count);
            foreach(var obj in linkObj_)
            {
                float length = (obj.transform.position - transform.position).magnitude;
                linkLength_.Add(length);
            }
        }
        //デバッグビュー
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            //接続しているオブジェクトと線を結ぶ
            foreach (var obj in linkObj_)
            {
                Gizmos.DrawLine(transform.position, obj.transform.position);
            }
        }

    }
}
