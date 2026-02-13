using System.Collections;
using System.Collections.Generic;
using Games_tutorial.Datasets;
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
            // control.AIR_CONTROL.SetFloat((int)AirControlFloat.AIR_MOMENTUM,0f);
            characterState.MOMENTUM_DATA.Momentum=0f;

            control.RIGID_BODY.velocity=Vector3.zero;
            
            if(control.ROTATION_DATA.IsFacingForward()){
                control.ROTATION_DATA.FaceForward(false);
            }else{
                control.ROTATION_DATA.FaceForward(true);
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