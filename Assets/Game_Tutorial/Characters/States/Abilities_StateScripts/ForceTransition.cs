using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "ForceTransition", menuName = "Games/AbilityData/ForceTransition")]
    public class ForceTransition : StateData
    {
        [Range(0.01f,1f)]
        public float TransitionTiming;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(stateInfo.normalizedTime>=TransitionTiming){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.ForceTransition],true);
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.ForceTransition],false);
        }
    }
}