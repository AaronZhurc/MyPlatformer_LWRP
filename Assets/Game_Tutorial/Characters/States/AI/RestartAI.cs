using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "RestartAI", menuName = "Games/AI/RestartAI")]
    public class RestartAI : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            //walking
            CharacterControl control=characterState.characterControl;
            if(control.aiProgress.AIDistanceToEndSphere()<1f){
                if(control.aiProgress.TargetDistanceToEndSphere()>0.5f){
                    if(control.aiProgress.TargetIsGrounded()){
                        control.aiController.InitializeAI();
                    }
                }
            }

            //landing
            if(control.animationProgress.IsRunning(typeof(Landing))){
                control.Turbo=false;
                control.Jump=false;
                control.MoveUp=false;
                control.aiController.InitializeAI();
            }

            //path blocked
            if(control.animationProgress.FrontBlockingObjs.Count==0){
                control.aiProgress.BlockCharacter=null;
            }else{
                foreach(KeyValuePair<GameObject,GameObject> data in control.animationProgress.FrontBlockingObjs){
                    CharacterControl blockingChar=CharacterManager.Instance.GetCharacter(data.Value);
                    if(blockingChar!=null){
                        control.aiProgress.BlockCharacter=blockingChar;
                        break;
                    }else{
                        control.aiProgress.BlockCharacter=null;
                    }
                }
            }
            if(control.aiProgress.BlockCharacter!=null){
                if(control.animationProgress.Ground!=null){
                    if(!control.animationProgress.IsRunning(typeof(Jump))
                       && !control.animationProgress.IsRunning(typeof(JumpPrep))){
                        control.Turbo=false;
                        control.Jump=false;
                        control.MoveUp=false;
                        control.MoveLeft=false;
                        control.MoveRight=false;
                        control.MoveDown=false;
                        control.aiController.InitializeAI();
                    }
                }
            }

            //startsphere height
            if(control.animationProgress.Ground!=null
               && !control.animationProgress.IsRunning(typeof(Jump))
               && !control.animationProgress.IsRunning(typeof(WallJump_Prep))){
                if(control.aiProgress.GetStartSphereHeight()>0.1f){
                    control.Turbo=false;
                    control.Jump=false;
                    control.MoveUp=false;
                    control.MoveLeft=false;
                    control.MoveRight=false;
                    control.MoveDown=false;
                    control.aiController.InitializeAI();
                }
            }
        }
    }
}