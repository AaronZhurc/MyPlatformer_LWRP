using System.Collections;
using System.Collections.Generic;
using Games_tutorial.Datasets;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "Idle", menuName = "Games/AbilityData/Idle")]
    public class Idle : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Jump],false);
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Attack],false);
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],false);

            //  CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            control.animationProgress.disallowEarlyTurn=false;

            // control.animationProgress.FrontBlockingObjs.Clear();
            // characterState.characterControl.ProcDic[CharacterProc.CLEAR_FRONTBLOCKINGOBJDIC]();
            characterState.BLOCKING_DATA.ClearFrontBlockingObjDic();
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            control.animationProgress.LockDirectionNextState=false;

            // if(control.animationProgress.AttackTriggered/*control.Attack*/){
            //     animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Attack],true);
            //     // characterState.characterControl.animationProgress.AttackTriggered=false;
            // }

            if(control.Jump){
                bool jumped=control.AIR_CONTROL.GetBool((int)AirControlBool.JUMPED);
                if(!jumped){
                    if(control.animationProgress.Ground!=null){
                        animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Jump],true);
                    }
                }
            }else{
                if(!control.animationProgress.IsRunning(typeof(Jump))){
                    control.AIR_CONTROL.SetBool((int)AirControlBool.JUMPED,false);
                }
            }
            if(control.MoveLeft&&control.MoveRight){
                //nothing need to do
            }else if(control.MoveRight){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],true);
            }else if(control.MoveLeft){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],true);
            }
            
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Attack],false);
        }
    }
}