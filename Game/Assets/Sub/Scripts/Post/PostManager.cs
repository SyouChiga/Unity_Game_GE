using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Game
{
    //ルート
    [Serializable]
    public class PostRoot
    {
        [SerializeField]
        //始まり
        private GameObject rootObj_;
        public GameObject StartRootObject
        {
            set
            {
                rootObj_ = value;
            }
            get
            {
                return rootObj_;
            }
        }
        
        [SerializeField]
        private List<PostGoal> rootGoalObj_  = new List<PostGoal>();
        public List<PostGoal> RootGoalObject
        {
            set
            {
                rootGoalObj_ = value;
            }
            get
            {
                return rootGoalObj_;
            }
        }

    }
    //ルートゴール
    [Serializable]
    public class PostGoal
    {
        [SerializeField]
        //終わりまでのルート
        private List<GameObject> goalRootObj_ = new List<GameObject>();
        public List<GameObject> GoalRootObject
        {
            set
            {
                goalRootObj_ = value;
            }
            get
            {
                return goalRootObj_;
            }
        }

    }

    public class PostManager : MonoBehaviour
    {
        //リンク
        [SerializeField]
        Link[] link_;
        //ルート
        [SerializeField]
        List<PostRoot> postRoot_;
        // Start is called before the first frame update
        void Start()
        {
            link_ = GameObject.FindObjectsOfType<Link>();

            Array.Sort(link_, (a, b) => a.LinkID - b.LinkID);

            Dijkstra();
        }

        // Update is called once per frame
        void Update()
        {

        }

        //ダイクストラ
        void Dijkstra()
        {
            PostRootInit();
        }

        //ポストルート初期化
        void PostRootInit()
        {
            //ポストルートのサイズを、Link変数分用意する
            foreach(var save in link_)
            {
                postRoot_.Add(new PostRoot());
            }
            int cnt = 0;
            foreach(var postRootObj in postRoot_)
            {
                postRootObj.StartRootObject = link_[cnt].gameObject;
                for (int nCnt = 0 ; nCnt < link_.Length; nCnt++)
                {
                    postRootObj.RootGoalObject.Add(new PostGoal());

                }
                cnt++;
            }
            PostRootRetrieval();
        }
        //ポストルートの検索
        void PostRootRetrieval()
        {
            foreach(var post in postRoot_)
            {
                int cnt = 0;
                foreach(var posGoal in postRoot_)
                {
                    //同じなら処理しない
                    if (post == posGoal)
                    {
                        cnt++;
                        continue;
                    }
                    post.RootGoalObject[cnt].GoalRootObject.Add(post.StartRootObject);
                    PostRootGoalReturn(post.StartRootObject.GetComponent<Link>(), posGoal.StartRootObject, post.RootGoalObject[cnt].GoalRootObject);
                    cnt++; 

                }
            }
        }
        //再帰　繋がっている先があるかどうか
        //trueなら次があり、falseは次がない
        bool PostRootGoalReturn(Link rLink,GameObject goal,List<GameObject> postRootGoal)
        {
            bool safe = true;
            foreach (var linkObj in rLink.LinkObject)
            {
                safe = true;
                int cnt = 0;
                foreach(var returnObj in postRootGoal)
                {
                    if(linkObj == returnObj)
                    {
                        if (linkObj == postRootGoal[0])
                        {
                            safe = false;
                        }
                        else
                        {
                            safe = false;
                            break;
                        }
                    }
                    cnt++;
                }
                if(goal == linkObj)
                {
                    postRootGoal.Add(linkObj);
                    return true;
                }
                if (safe == true)
                {
                    postRootGoal.Add(linkObj);
                    if(PostRootGoalReturn(linkObj.GetComponent<Link>(), goal, postRootGoal))
                    {
                        return true;
                    }
                    else
                    {
                         
                        if(postRootGoal.Count - 1 != 0) postRootGoal.RemoveAt(postRootGoal.Count - 1);
                    }
                }
               
            }
            return safe;
        }




    }
}
