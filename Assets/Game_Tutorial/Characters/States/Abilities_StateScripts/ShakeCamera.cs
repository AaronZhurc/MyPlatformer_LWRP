using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "ShakeCamera", menuName = "Games/AbilityData/ShakeCamera")]
    public class ShakeCamera : StateData
    {      
        [Range(0f,0.99f)]
        public float ShakeTiming;
        public float SkakeLength;
        //private bool isShaken=false; //应当为单个角色的状态而非通用
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(ShakeTiming==0f){
                // CharacterControl control=characterState.GetCharacterControl(animator);
                CharacterControl control=characterState.characterControl;

                CameraManager.Instance.ShakeCamera(SkakeLength);
                //isShaken=true;
                control.animationProgress.CameraShaken=true;
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            if(!control.animationProgress.CameraShaken){
                if(stateInfo.normalizedTime>=ShakeTiming){
                    control.animationProgress.CameraShaken=true;
                    CameraManager.Instance.ShakeCamera(SkakeLength);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            control.animationProgress.CameraShaken=false;
        }
    }
}