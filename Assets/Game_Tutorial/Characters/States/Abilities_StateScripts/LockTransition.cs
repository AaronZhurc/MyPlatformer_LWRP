using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "LockTransition", menuName = "Games/AbilityData/LockTransition")]
    public class LockTransition : StateData
    {
        public float UnlockTime;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control=characterState.characterControl;
            control.animationProgress.LockTransition=true;

        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control=characterState.characterControl;
            if(stateInfo.normalizedTime>UnlockTime){
                control.animationProgress.LockTransition=false;
            }else{
                control.animationProgress.LockTransition=true;
            }
        }
    }
}