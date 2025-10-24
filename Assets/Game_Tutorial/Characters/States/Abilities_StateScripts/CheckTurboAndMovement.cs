using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "CheckTurboAndMovement", menuName = "Games/AbilityData/CheckTurboAndMovement")]
    public class CheckTurboAndMovement : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            if((control.MoveLeft||control.MoveRight)&&control.Turbo){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],true);
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turbo],true);
            }else{
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],false);
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turbo],false);
            }
            
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }
}
