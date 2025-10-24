using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "WeaponPutDown", menuName = "Games/AbilityData/WeaponPutDown")]

    public class WeaponPutDown : StateData
    {
        public float PutDownTiming;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }   

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control=characterState.characterControl;
            if(stateInfo.normalizedTime>PutDownTiming){
                if(control.animationProgress.HoldingWeapon!=null){
                    control.animationProgress.HoldingWeapon.DropWeapon();
                }
            }
        }
    }
}