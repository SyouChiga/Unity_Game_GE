using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Character
    {
        namespace Enemy
        {
            public class BaseEnemy : BaseCharacter
            {
                protected GameObject[] postObj_;
                public GameObject[] PostObject
                {
                    get
                    {
                        return postObj_;
                    }
                }
                protected Animation.EnemyAnimation enemyAnimation_;
                public Animation.EnemyAnimation Animation
                {
                    get
                    {
                        return enemyAnimation_;
                    }
                }
                protected BaseEnemyState state_;
                public BaseEnemyState State
                {
                    get
                    {
                        return state_;
                    }
                    set
                    {
                        state_ = value;
                    }
                }

                // Start is called before the first frame update
                void Start()
                {
                    Init();
                }

                // Update is called once per frame
                void Update()
                {

                }

                //Init
                protected void Init()
                {
                    state_ = gameObject.AddComponent<EnemyWaitState>();
                    enemyAnimation_ = GetComponent<Animation.EnemyAnimation>();
                    GameObject[] post = GameObject.FindGameObjectsWithTag("post_tag");
                    postObj_ = post;
                }
            }
        }
    }
}
