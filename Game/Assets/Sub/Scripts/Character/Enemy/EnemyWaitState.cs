using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class EnemyWaitState : BaseEnemyState
            {
                [SerializeField]
                private GameObject goalPost_;
                [SerializeField]
                private float waitTime_;
                // Start is called before the first frame update
                void Start()
                {
                   
                    PostRoot();
                }

                // Update is called once per frame
                void Update()
                {
                    waitTime_ += Time.deltaTime;
                    if(waitTime_ >= 3.0f)
                    {
                        Object.Destroy(GetComponent<BaseEnemy>().State);
                        EnemyWalkState walk= gameObject.AddComponent<EnemyWalkState>();
                        walk.GoalPosition = goalPost_.transform;
                        GetComponent<BaseEnemy>().State = walk;
                        GetComponent<BaseEnemy>().Animation.Walk = true;
                    }
                }

                //目的地を決める
                private void PostRoot()
                {
                    BaseEnemy enemy = GetComponent<BaseEnemy>();

                    GameObject[] postObj = enemy.PostObject;

                    int range = Random.Range(0, postObj.Length);

                    goalPost_ = postObj[range];
                }
            }
        }
    }
}
