using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "CheckRunningTurn", menuName = "Games/AbilityData/CheckRunningTurn")]
    public class CheckRunningTurn : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            if(control.IsFacingForward()){
                if(control.MoveLeft){
                    animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turn],true);
                }
            }else{
                if(control.MoveRight){
                    animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turn],true);
                }
            }
            
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turn],false);
        }
    }
}
