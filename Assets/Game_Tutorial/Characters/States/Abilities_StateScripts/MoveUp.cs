using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "MoveUp", menuName = "Games/AbilityData/MoveUp")]
    public class MoveUp : StateData
    {
        public AnimationCurve SpeedGraph;
        public float Speed;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
             CharacterControl control=characterState.characterControl;
            
            characterState.characterControl.animationProgress.LatestMoveUp=this;
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control=characterState.characterControl;
            if(!characterState.characterControl.RIGID_BODY.useGravity) {
                // if(!UpIsBlocked(control)){
                //     control.transform.Translate(Vector3.up*Speed*SpeedGraph.Evaluate(stateInfo.normalizedTime)*Time.deltaTime);
                // }
                if(control.animationProgress.UpBlockingObjs.Count==0) {
                    control.transform.Translate(Vector3.up * Speed * SpeedGraph.Evaluate(stateInfo.normalizedTime) * Time.deltaTime);
                }
            }
        }

    //     bool UpIsBlocked(CharacterControl control){
    //         foreach(GameObject o in control.collisionSpheres.UpSpheres){
    //             Debug.DrawRay(o.transform.position,control.transform.up*0.3f,Color.yellow);
    //             RaycastHit hit;
    //             if(Physics.Raycast(o.transform.position,control.transform.up,out hit,0.125f)){ //使用射线检测距离
    //                 if(hit.collider.transform.root.gameObject!=control.gameObject
    //                    &&!Ledge.IsLedge(hit.collider.gameObject)){ 
    //                     return true;
    //                 }
    //             }
    //         }
    //         return false;
    //     }
    }
}