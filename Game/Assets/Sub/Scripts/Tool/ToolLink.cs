using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    namespace Tool
    {
        public class ToolLink : BaseTool
        {
            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {

            }

#if UNITY_EDITOR
            [MenuItem("Tools/LINK %g")]
            //リンクの再接続
            public static void ReturnLink()
            {
                //Scene上にあるリンクを拾う
                
                Link[] links = GameObject.FindObjectsOfType<Link>();
                
                foreach (var link in links)
                {
                    //接続されているオブジェクト
                    GameObject[] linkObj = link.LinkObject;
                   
                    foreach (var obj in linkObj)
                    {
                        //接続先のオブジェクトのリンク
                        Link nextLink = obj.GetComponent<Link>();
                        
                        bool safe = false; //接続先の自分が存在しているかどうか
                        GameObject[] Object = nextLink.LinkObject;
                        foreach (var nextLinkObj in nextLink.LinkObject)
                        {
                            
                            if (nextLink == obj)
                            {
                                safe = true;
                                break;
                            }
                        }
                       
                        //接続先に自分が存在していなかったら
                        if (!safe)
                        {
                            Debug.Log(safe);
                            GameObject[] saveObject = new GameObject[nextLink.LinkObject.Length + 1];
                            int cnt = 0; //カウント
                            for(cnt = 0; cnt < nextLink.LinkObject.Length; cnt++)
                            {
                                saveObject[cnt] = nextLink.LinkObject[cnt];
                            }
                            //接続先に自分自身を入れる
                            saveObject[cnt] = link.gameObject;
                            nextLink.LinkObject = saveObject;
                        }
                    }
                }
            }
#endif
        }
    }
}
