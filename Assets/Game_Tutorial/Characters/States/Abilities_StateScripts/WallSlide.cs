using System.Collections;
using System.Collections.Generic;
using Games_tutorial.Datasets;
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

            // control.AIR_CONTROL.SetFloat((int)AirControlFloat.AIR_MOMENTUM,0f);
            characterState.MOMENTUM_DATA.Momentum=0f;
            // control.AIR_CONTROL.SetBool((int)AirControlBool.CAN_WALL_JUMP,false);
            control.JUMP_DATA.CanWallJump=false;

            characterState.VERTICAL_VELOCITY_DATA.MaxWallSideVelocity=MaxFallVelocity;
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.VERTICAL_VELOCITY_DATA.MaxWallSideVelocity=Vector3.zero;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(!characterState.characterControl.Jump){
                // characterState.characterControl.AIR_CONTROL.SetBool((int)AirControlBool.CAN_WALL_JUMP,true);
                characterState.characterControl.JUMP_DATA.CanWallJump=true;
            }
        }
    }
}