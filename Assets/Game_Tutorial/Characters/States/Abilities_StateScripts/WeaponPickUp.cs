using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "WeaponPickUp", menuName = "Games/AbilityData/WeaponPickUp")]

    public class WeaponPickUp : StateData
    {
        public float PickUpTiming;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control=characterState.characterControl;
            if(stateInfo.normalizedTime>PickUpTiming){
                if(control.animationProgress.HoldingWeapon==null){
                    Weapon w=control.animationProgress.GetTouchingWeapon();;
                    characterState.characterControl.animationProgress.HoldingWeapon=w;
                
                    w.transform.parent=control.RightHand_Attack.transform;
                    w.transform.localPosition=w.CustomPosition;
                    w.transform.localRotation=Quaternion.Euler(w.CustomRotation);
                
                    w.control=control;
                    w.triggerDetector.control=control;
                }
            }
        }
    }
}