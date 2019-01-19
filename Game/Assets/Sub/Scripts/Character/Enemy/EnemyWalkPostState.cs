using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class EnemyWalkPostState : BaseEnemyState
            {
                //ゴールまでの道筋
                [SerializeField]
                List<GameObject> postGoal_;
                public List<GameObject> postGoal
                {
                    set
                    {
                        postGoal_ = value;
                    }
                    get
                    {
                        return postGoal_;
                    }
                }
                //ゴールまでのカウント
                int cntGoal_;
                public int CntGoal
                {
                    set
                    {
                        cntGoal_ = value;
                    }
                }

                // Start is called before the first frame update
                void Start()
                {

                }

                // Update is called once per frame
                void Update()
                {
                    if(Walk())
                    {
                        cntGoal_++;
                        //まだ目的地についていないなら
                        if(cntGoal_ < postGoal_.Count)
                        {
                            Object.Destroy(GetComponent<BaseEnemy>().State);
                            EnemyWalkPostState walk = gameObject.AddComponent<EnemyWalkPostState>();
                            walk.postGoal = postGoal_;
                            walk.CntGoal = cntGoal_;
                            GetComponent<BaseEnemy>().State = walk;
                            GetComponent<BaseEnemy>().Animation.Walk = true;
                        }
                        else
                        {
                            Object.Destroy(GetComponent<BaseEnemy>().State);
                            GetComponent<BaseEnemy>().State = gameObject.AddComponent<EnemyWaitState>();
                            GetComponent<BaseEnemy>().Animation.Walk = false;
                        }
                    }
                }

                //目的地まで向かう
                bool Walk()
                {
                    Vector3 goalVectorXZ = new Vector3(postGoal_[cntGoal_].transform.position.x, 0.0f, postGoal_[cntGoal_].transform.position.z);
                    Vector3 enemyVectorXZ = new Vector3(transform.position.x, 0.0f, transform.position.z);

                    Vector3 goalVector = (goalVectorXZ - enemyVectorXZ);

                    transform.position += goalVector.normalized * 0.05f;


                    //if (transform.position.y != postGoal_[cntGoal_].transform.position.y)
                    //{
                    //    postGoal_[cntGoal_].transform.position = new Vector3(postGoal_[cntGoal_].transform.position.x, transform.position.y, postGoal_[cntGoal_].transform.position.z);
                    //}
                    Quaternion targetRotation = Quaternion.LookRotation(postGoal_[cntGoal_].transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.05f);
                    if (goalVector.magnitude < 1.0f)
                    {
                        return true;
                    }


                    return false;
                }
            }
        }
    }
}
