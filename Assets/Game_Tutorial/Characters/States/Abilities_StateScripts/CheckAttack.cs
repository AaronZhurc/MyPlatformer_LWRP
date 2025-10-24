using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "CheckAttack", menuName = "Games/AbilityData/CheckAttack")]
    public class CheckAttack : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);

            if(characterState.characterControl.animationProgress.AttackTriggered/*control.Attack*/){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Attack],true);
                // characterState.characterControl.animationProgress.AttackTriggered=false;
            }
            
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }
}