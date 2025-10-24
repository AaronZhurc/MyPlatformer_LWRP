using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "GravityPull", menuName = "Games/AbilityData/GravityPull")]
    public class GravityPull : StateData
    {
        public AnimationCurve Gravity;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            // CharacterControl control=characterState.characterControl;
            // control.GravityMultipilier=Gravity.Evaluate(stateInfo.normalizedTime);
            
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            // CharacterControl control=characterState.characterControl;
            // control.GravityMultipilier=0f;
        }
    }
}