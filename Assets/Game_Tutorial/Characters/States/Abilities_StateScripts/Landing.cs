using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "Landing", menuName = "Games/AbilityData/Landing")]
    public class Landing : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // characterState.characterControl.animationProgress.IsLanding=true;
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Jump],false);
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],false); //着陆时不应该继续移动了
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // characterState.characterControl.animationProgress.IsLanding=false;
        }
    }
}