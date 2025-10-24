using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "ToggleGravity", menuName = "Games/AbilityData/ToggleGravity")]
    public class ToggleGravity : StateData
    {
        public bool On;
        public bool OnStart;
        public bool OnEnd;
        public float CustomTiming;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(OnStart){
                // CharacterControl control=characterState.GetCharacterControl(animator);
                CharacterControl control=characterState.characterControl;
                ToggleGrav(control);
            }
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(CustomTiming!=0f){
                if(CustomTiming<=stateInfo.normalizedTime){
                    CharacterControl control=characterState.characterControl;
                    ToggleGrav(control);
                }
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(OnEnd){
                // CharacterControl control=characterState.GetCharacterControl(animator);
                CharacterControl control=characterState.characterControl;
                ToggleGrav(control);
            }
        }
        private void ToggleGrav(CharacterControl control){
            control.RIGID_BODY.velocity=Vector3.zero;
            control.RIGID_BODY.useGravity=On;
        }
    }
}