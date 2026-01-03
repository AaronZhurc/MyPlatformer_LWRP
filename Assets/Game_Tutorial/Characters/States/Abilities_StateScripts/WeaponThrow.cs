using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "WeaponThrow", menuName = "Games/AbilityData/WeaponThrow")]

    public class WeaponThrow : StateData
    {
        public float ThrowTiming;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }   

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control=characterState.characterControl;
            if(stateInfo.normalizedTime>ThrowTiming){
                if(control.animationProgress.HoldingWeapon!=null){
                    control.animationProgress.HoldingWeapon.ThrowWeapon();
                }
            }
        }
    }
}