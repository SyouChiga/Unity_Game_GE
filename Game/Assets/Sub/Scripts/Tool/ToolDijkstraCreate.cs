using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


namespace Game
{
    namespace Tool
    {
#if UNITY_EDITOR
        public class ToolDijkstraCreate : EditorWindow
        {
            //リンク
            [SerializeField]
            static Link[] link_;
            //ルート
            [SerializeField]
            static List<PostRoot> postRoot_;
            //ダイクストラのセーブ
            Save.DijkstraSave dijkstraSave_;

            [MenuItem("Tools/DijkstraCreate %d")]
            static void Init()
            {
                EditorWindow.GetWindow<ToolDijkstraCreate>(true, "DijkstraCreate");
            }
            void OnGUI()
            {
                dijkstraSave_ = EditorGUILayout.ObjectField("SAVE", dijkstraSave_, typeof(Save.DijkstraSave), true) as Save.DijkstraSave;
                if (GUILayout.Button("Create"))
                {
                    CreateInit();
                }

                EditorUtility.SetDirty(dijkstraSave_);
            }

            void CreateInit()
            {
                link_ = GameObject.FindObjectsOfType<Link>();

                Array.Sort(link_, (a, b) => a.LinkID - b.LinkID);

                Dijkstra();
                DijkstraSave();
            }

            //ダイクストラ
            void Dijkstra()
            {
                PostRootInit();
            }

            //ポストルート初期化
            void PostRootInit()
            {
                postRoot_ = new List<PostRoot>(link_.Length);
                //ポストルートのサイズを、Link変数分用意する
                foreach (var save in link_)
                {
                    postRoot_.Add(new PostRoot());
                }
                int cnt = 0;
                foreach (var postRootObj in postRoot_)
                {
                    postRootObj.StartRootObject = link_[cnt].gameObject;
                    postRootObj.RootGoalObject = new List<PostGoal>();
                    for (int nCnt = 0; nCnt < link_.Length; nCnt++)
                    {
                        postRootObj.RootGoalObject.Add(new PostGoal());
                        postRootObj.RootGoalObject[nCnt].GoalRootObject = new List<GameObject>();
                    }
                    cnt++;
                }
                PostRootRetrieval();
            }
            //ポストルートの検索
            void PostRootRetrieval()
            {
                foreach (var post in postRoot_)
                {
                    int cnt = 0;
                    foreach (var posGoal in postRoot_)
                    {


                        float length_, lenght1_;
                        bool triger = false;
                        length_ = lenght1_ = 0.0f;
                        List<GameObject> goalSaveRootObj_ = new List<GameObject>();
                        goalSaveRootObj_.Add(post.StartRootObject);
                        post.RootGoalObject[cnt].GoalRootObject.Add(post.StartRootObject);
                        bool one = false;
                        //同じなら処理しない
                        if (post == posGoal)
                        {
                            cnt++;
                            continue;
                        }
                        //隣にあるかどうか調べる
                        foreach (var linkObj in post.StartRootObject.GetComponent<Link>().LinkObject)
                        {
                            if (linkObj == posGoal.StartRootObject)
                            {
                                one = true;
                                post.RootGoalObject[cnt].GoalRootObject.Add(linkObj);
                                break;
                            }
                        }

                        if (one == false) PostRootGoalReturn(post.StartRootObject.GetComponent<Link>(), posGoal.StartRootObject, post.RootGoalObject[cnt].GoalRootObject, goalSaveRootObj_, ref length_, ref lenght1_, ref triger);
                        cnt++;

                    }
                }
            }
            //再帰　繋がっている先があるかどうか
            //trueなら次があり、falseは次がない
            bool PostRootGoalReturn(Link rLink, GameObject goal, List<GameObject> postRootGoal, List<GameObject> postRootNewGoal, ref float length, ref float newLength, ref bool triger)
            {
                bool safe = true;
                int index = 0;
                foreach (var linkObj in rLink.LinkObject)
                {
                    safe = true;
                    int cnt = 0;
                    foreach (var returnObj in postRootNewGoal)
                    {
                        if (linkObj == returnObj)
                        {
                            if (linkObj == postRootNewGoal[0])
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
                    if (goal == linkObj)
                    {
                        if (index >= 1)
                        {
                            if (postRootNewGoal[postRootNewGoal.Count - 1] == rLink.LinkObject[index - 1])
                            {
                                if (postRootNewGoal.Count - 1 != 0) postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);
                            }
                        }
                        postRootNewGoal.Add(linkObj);
                        return true;
                    }
                    if (safe == true)
                    {
                        postRootNewGoal.Add(linkObj);

                        if (PostRootGoalReturn(linkObj.GetComponent<Link>(), goal, postRootGoal, postRootNewGoal, ref length, ref newLength, ref triger))
                        {

                            for (int cntObj = postRootNewGoal.Count - 1; cntObj > 0; cntObj--)
                            {
                                newLength += (postRootNewGoal[cntObj - 1].transform.position - postRootNewGoal[cntObj].transform.position).magnitude;
                            }

                            //前回の経路探索を見比べる
                            if (newLength <= length || triger == false)
                            {
                                triger = true;
                                postRootGoal.Clear();
                                foreach (var saveObj in postRootNewGoal)
                                {
                                    postRootGoal.Add(saveObj);
                                }
                                length = newLength;


                            }
                            newLength = 0.0f;
                            if (postRootNewGoal.Count - 1 != 0) postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);
                            if (postRootNewGoal.Count - 1 != 0) postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);

                        }
                        else
                        {

                            if (postRootNewGoal.Count - 1 != 0 && rLink.gameObject != postRootNewGoal[postRootNewGoal.Count - 1])
                            {
                                postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);
                            }
                        }
                    }
                    index++;


                }
                if (postRootNewGoal.Count - 1 != 0)
                {
                    postRootNewGoal.RemoveAt(postRootNewGoal.Count - 1);

                }

                return false;
            }

            //ダイクストラセーブ
            void DijkstraSave()
            {
                List<Save.PostRootINT> postRootSave_;
                postRootSave_ = dijkstraSave_.LinkSave;

                postRootSave_.Clear();

                //リンクIDを入れる
                for (int cnt = 0; cnt < postRoot_.Count; cnt++)
                {
                    postRootSave_.Add(new Save.PostRootINT());
                    postRootSave_[cnt].StartRootObject = postRoot_[cnt].StartRootObject.GetComponent<Link>().LinkID;
                    postRootSave_[cnt].RootGoalObject = new List<Save.PostGoalINT>(postRoot_.Count);

                    for (int cntIndex = 0; cntIndex < postRoot_.Count; cntIndex++)
                    {
                        postRootSave_[cnt].RootGoalObject.Add(new Save.PostGoalINT());
                        postRootSave_[cnt].RootGoalObject[cntIndex] = new Save.PostGoalINT();
                        postRootSave_[cnt].RootGoalObject[cntIndex].GoalRootObject = new List<int>();
                    }
                    int index = 0;
                    foreach (var linkObj in postRoot_[cnt].RootGoalObject)
                    {
                        foreach (var goalObj in linkObj.GoalRootObject)
                        {
                            postRootSave_[cnt].RootGoalObject[index].GoalRootObject.Add(goalObj.GetComponent<Link>().LinkID);

                        }
                        index++;
                    }
                }


            }



        }
#endif

    }
}

