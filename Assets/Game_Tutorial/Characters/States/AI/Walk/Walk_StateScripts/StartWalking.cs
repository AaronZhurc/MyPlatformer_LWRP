using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "StartWalking", menuName = "Games/AI/StartWalking")]
    public class StartWalking : StateData
    {
       
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.characterControl;
            
            control.aiProgress.SetRandomFlyingKick();
            control.aiController.WalkStraightToStartSphere();
            // WalkStraightTowardsTarget(control);
        
        }


        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            // Vector3 dist=control.aiProgress.pathfindingAgent.StartSphere.transform.position-control.transform.position;
            if(control.Attack){
                return;
            }

            //jump
            if (control.aiProgress.EndSphereIsHigher()){
                if(control.aiProgress.AIDistanceToStartSphere()<0.08f){
                    control.MoveRight=false;
                    control.MoveLeft=false;   
                    animator.SetBool(HashManager.Instance.DicAITrans[AI_Walk_Transitions.jump_platform],true);
                    return;
                }
            }
            //fall
            if (control.aiProgress.EndSphereIsLower()){
                control.aiController.WalkStraightToEndSphere();
                animator.SetBool(HashManager.Instance.DicAITrans[AI_Walk_Transitions.fall_platform],true);
                return;
            }

            //straight
            if(control.aiProgress.AIDistanceToStartSphere()>1.5f){
                control.Turbo=true;
            }else{
                control.Turbo=false;
            }

            control.aiController.WalkStraightToStartSphere();

            if(control.aiProgress.AIDistanceToEndSphere()<1f){
                control.Turbo=false;
                control.MoveRight=false;
                control.MoveLeft =false;
            }

            if(control.aiProgress.TargetIsOnSamePlatform()){
                control.aiProgress.RepositionDestination();
            }

            // if (control.aiProgress.pathfindingAgent.StartSphere.transform.position.y
            //     ==control.aiProgress.pathfindingAgent.EndSphere.transform.position.y){
            //      if(control.aiProgress.GetDistanseToDestination()<0.5f){
            //         control.MoveRight=false;
            //         control.MoveLeft=false;   

            //         Vector3 playerDist=control.transform.position-CharacterManager.Instance.GetPlayableCharacter().transform.position;
            //         if(playerDist.sqrMagnitude>1f){ //玩家移动走了，临时方案 
            //             animator.gameObject.SetActive(false);
            //             animator.gameObject.SetActive(true);
            //         }       
            //     }
            // }

        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.DicAITrans[AI_Walk_Transitions.jump_platform],false);
            animator.SetBool(HashManager.Instance.DicAITrans[AI_Walk_Transitions.fall_platform],false);
        }
    }
}