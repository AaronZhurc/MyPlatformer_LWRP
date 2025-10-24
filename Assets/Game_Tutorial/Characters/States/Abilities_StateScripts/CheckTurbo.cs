using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "CheckTurbo", menuName = "Games/AbilityData/CheckTurbo")]
    public class CheckTurbo : StateData
    {
        public bool MustRequireMovement;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            if(control.Turbo){
                if(MustRequireMovement){
                    if(control.MoveLeft||control.MoveRight){
                        animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turbo],true);
                    }else{
                        animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turbo],false);
                    }
                }else{
                    animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turbo],true);
                }
            }else{
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turbo],false);
            }
            
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }
}