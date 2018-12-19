using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        public class EnemyAnimation : BaseAnimation
        {
            [SerializeField]
            private bool walk_ = false; //歩きフラグ
            public bool Walk
            {
                get
                {
                    return walk_;
                }
                set
                {
                    walk_ = value;
                }
            }
            void Start()
            {
                animationState_ = gameObject.AddComponent<Enemy.EnemyWaitState>();
                animationState_.Animation = this;
                anim = GetComponent<Animator>();
            }
            void Update()
            {
                AnimationState();
            }
            //アニメーションステート
            new public void AnimationState()
            {

            }
        }
    }
}
