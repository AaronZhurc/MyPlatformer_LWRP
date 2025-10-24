using System.Collections;
using System.Collections.Generic;
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
            control.animationProgress.Jumped=false;
            if(JumpTiming==0f){
                control.RIGID_BODY.AddForce(Vector3.up*JumpForce);
                control.animationProgress.Jumped=true;
                //isJumped=true;
            }
            //animator.SetBool(TransitionParameter.Grounded.ToString(),false);
            control.animationProgress.CancelPull=CancelPull;
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            //control.GravityMultipilier=Gravity.Evaluate(stateInfo.normalizedTime);
            //control.PullMultipilier=Pull.Evaluate(stateInfo.normalizedTime);
            if(!control.animationProgress.Jumped&&stateInfo.normalizedTime>=JumpTiming){
                control.RIGID_BODY.AddForce(Vector3.up*JumpForce);
                control.animationProgress.Jumped=true;
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