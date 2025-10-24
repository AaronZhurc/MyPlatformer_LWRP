using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "OffsetOnLedge", menuName = "Games/AbilityData/OffsetOnLedge")]
    public class OffsetOnLedge : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            // CharacterControl control=characterState.characterControl;
            // GameObject anim=control.SkinnedMeshAnimator.gameObject;
            // anim.transform.parent=control.ledgeChecker.GrabbedLedge.transform;
            // anim.transform.localPosition=control.ledgeChecker.GrabbedLedge.Offset;

            // float x,y,z;
            // if(control.IsFacingForward()){
            //     x=control.ledgeChecker.LedgeCalibration.x;
            //     y=control.ledgeChecker.LedgeCalibration.y;
            //     z=control.ledgeChecker.LedgeCalibration.z;
            // }else{
            //     x=control.ledgeChecker.LedgeCalibration.x;
            //     y=control.ledgeChecker.LedgeCalibration.y;
            //     z=-control.ledgeChecker.LedgeCalibration.z;
            // }

            // Vector3 calibration;
            // calibration.x=x;
            // calibration.y=y;
            // calibration.z=z;

            // anim.transform.localPosition+=calibration;

            // control.RIGID_BODY.velocity=Vector3.zero;
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.characterControl;
            // if(!control.RIGID_BODY.useGravity){
            //     control.RIGID_BODY.MovePosition(control.ledgeChecker.GrabbedLedge.transform.position+control.ledgeChecker.GrabbedLedge.Offset);
            // }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
             
        }
    }
}