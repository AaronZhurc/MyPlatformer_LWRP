using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "WallSlide", menuName = "Games/AbilityData/WallSlide")]
    public class WallSlide : StateData
    {
        public Vector3 MaxFallVelocity;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control=characterState.characterControl;

            control.MoveLeft=false;
            control.MoveRight=false;
            control.animationProgress.AirMomentum=0f;

            control.animationProgress.MaxFallVelocity=MaxFallVelocity;
            control.animationProgress.CanWallJump=false;
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.animationProgress.MaxFallVelocity=Vector3.zero;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(!characterState.characterControl.Jump){
                characterState.characterControl.animationProgress.CanWallJump=true;
            }
        }
    }
}