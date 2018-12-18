﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    namespace Animation
    {
        namespace Player
        {
            public class PlayerWaitState : BasePlayerAnimationState
            {
                // Start is called before the first frame update
                void Start()
                {
                    playerAnimation = (PlayerAnimation)animation;

                    playerAnimation.Animator.SetBool("is_walk", false);
                }

                // Update is called once per frame
                void Update()
                {
                    if(playerAnimation.Walk)
                    {
                    
                        AddAnimation();
                        
                    }
                }

                //アニメーションコンポーネントの追加
                new public void AddAnimation()
                {
                   Object.Destroy(playerAnimation.AnimationStateValue);
                   playerAnimation.AnimationStateValue = gameObject.AddComponent<PlayerWalkState>();
                   playerAnimation.ChangeAnimationState();
                }
            }
        }
    }
}
