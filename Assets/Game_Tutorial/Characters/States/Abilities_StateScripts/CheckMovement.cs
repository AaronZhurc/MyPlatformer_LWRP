using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "CheckMovement", menuName = "Games/AbilityData/CheckMovement")]
    public class CheckMovement : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            if(control.MoveLeft||control.MoveRight){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],true);
            }else{
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],false);
            }
            
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }
}
