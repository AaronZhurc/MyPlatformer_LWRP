using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "WallJump_Prep", menuName = "Games/AbilityData/WallJump_Prep")]

    public class WallJump_Prep : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control=characterState.characterControl;
            
            control.MoveLeft=false;
            control.MoveRight=false;
            control.animationProgress.AirMomentum=0f;

            control.RIGID_BODY.velocity=Vector3.zero;
            
            if(control.IsFacingForward()){
                control.FaceForward(false);
            }else{
                control.FaceForward(true);
            }

        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }
}