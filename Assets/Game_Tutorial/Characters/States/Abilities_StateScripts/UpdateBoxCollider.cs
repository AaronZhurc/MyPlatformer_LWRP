using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "UpdateBoxCollider", menuName = "Games/AbilityData/UpdateBoxCollider")]

    public class UpdateBoxCollider:StateData {
        public Vector3 TargetCenter;
        public float CenterUpdateSpeed;
        [Space(10)]
        public Vector3 TargetSize;
        public float SizeUpdateSpeed;
        // [Space(10)]
        // public bool KeepUpdating;

        const string LandingState = "Jump_Normal_Landing";
        const string ClimbingState = "LedgeClimb";

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo) {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control = characterState.characterControl;
            // control.animationProgress.UpdatingBoxCollider=true;

            control.animationProgress.TargetSize = TargetSize;
            control.animationProgress.Size_Speed = SizeUpdateSpeed;
            control.animationProgress.TargetCenter = TargetCenter;
            control.animationProgress.Center_Speed = CenterUpdateSpeed;

            if(stateInfo.IsName(LandingState)) {
                characterState.characterControl.animationProgress.IsLanding = true;
            }
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo) {
            if(stateInfo.IsName(ClimbingState)) {
                if(stateInfo.normalizedTime > 0.7f) {
                    if(animator.GetBool(HashManager.Instance.DicMainParams[TransitionParameter.Grounded]) == true) {
                        characterState.characterControl.animationProgress.IsLanding = true;
                    }
                    else {
                        characterState.characterControl.animationProgress.IsLanding = false;
                    }
                }
                else {
                    characterState.characterControl.animationProgress.IsLanding = false;
                }
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo) {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            // CharacterControl control=characterState.characterControl;
            // if(!KeepUpdating){
            //     control.animationProgress.UpdatingBoxCollider=false;
            // }
            if(stateInfo.IsName(LandingState)||stateInfo.IsName(ClimbingState)) {
                characterState.characterControl.animationProgress.IsLanding = false;
            }
        }
    }
}