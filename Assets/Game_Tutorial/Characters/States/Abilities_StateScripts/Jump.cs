using System.Collections;
using System.Collections.Generic;
using Games_tutorial.Datasets;
using UnityEngine;
using UnityEngine.AI;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "Jump", menuName = "Games/AbilityData/Jump")]
    public class Jump : StateData
    {
        [Range(0f,1f)]
        public float JumpTiming;
        public float JumpForce;
        //public AnimationCurve Gravity;
        [Header("Extra Gravity")]
        // public AnimationCurve Pull;
        //private bool isJumped; //此变量倾向于特定人物，而不是通用性质
        public bool CancelPull;
        

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            // control.AIR_CONTROL.SetBool((int)AirControlBool.JUMPED,false);
            characterState.JUMP_DATA.Jumped=false;
            if(JumpTiming==0f){
                control.RIGID_BODY.AddForce(Vector3.up*JumpForce);
                // control.AIR_CONTROL.SetBool((int)AirControlBool.JUMPED,true);
                characterState.JUMP_DATA.Jumped=true;
                //isJumped=true;
            }
            //animator.SetBool(TransitionParameter.Grounded.ToString(),false);
            characterState.VERTICAL_VELOCITY_DATA.NoJumpCancel=CancelPull;
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            // bool jumped=control.AIR_CONTROL.GetBool((int)AirControlBool.JUMPED);
            //control.GravityMultipilier=Gravity.Evaluate(stateInfo.normalizedTime);
            //control.PullMultipilier=Pull.Evaluate(stateInfo.normalizedTime);
            if(!characterState.JUMP_DATA.Jumped&&stateInfo.normalizedTime>=JumpTiming){
                control.RIGID_BODY.AddForce(Vector3.up*JumpForce);
                characterState.JUMP_DATA.Jumped=true;
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            // CharacterControl control=characterState.characterControl;
            // // control.PullMultipilier=0f;
            // control.animationProgress.Jumped=false;
        }
    }
}