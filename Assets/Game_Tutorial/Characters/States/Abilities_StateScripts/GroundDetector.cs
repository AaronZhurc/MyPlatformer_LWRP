using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "GroundDetector", menuName = "Games/AbilityData/GroundDetector")]
    public class GroundDetector : StateData
    {
        //[Range(0.01f,1f)]
        //public float CheckTime;
        public float Distance;

        private GameObject testingSphere;
        public GameObject TESTING_SPHERE
        {
            get
            {
                if (testingSphere == null)
                {
                    testingSphere = GameObject.Find("TestingSphere");
                }
                return testingSphere;
            }
        }

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            //if(stateInfo.normalizedTime>=CheckTime){
            if(IsGrounded(control)){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Grounded],true);
            }else{
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Grounded],false);
            }
            //}
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        bool IsGrounded(CharacterControl control){
            //if(control.RIGID_BODY.velocity.y>-0.001f&&control.RIGID_BODY.velocity.y<=0f){ //使用速度进行判断，但由于有时这个速度不是严格0，所以我们得弄出区间
                if(control.contactPoints!=null){
                    foreach(ContactPoint c in control.contactPoints){
                        float colliderBottom=(control.transform.position.y+control.boxCollider.center.y)-(control.boxCollider.size.y/2f);
                        float yDifference=Mathf.Abs(c.point.y-colliderBottom);
                        if(yDifference<0.01f){
                            if(Math.Abs(control.RIGID_BODY.velocity.y)<0.001f){
                                control.animationProgress.Ground=c.otherCollider.transform.root.gameObject;
                                control.animationProgress.LandingPosition=new Vector3(0f,c.point.y,c.point.z);
                                if (control.manualInput.enabled)
                                {
                                    TESTING_SPHERE.transform.position = control.animationProgress.LandingPosition;
                                }
                            return true;
                            }
                        }
                    }
                }
            //}

            if(control.RIGID_BODY.velocity.y<0f){//只有下落时才检查
                foreach(GameObject o in control.collisionSpheres.BottomSpheres){
                    Debug.DrawRay(o.transform.position,-Vector3.up*Distance,Color.yellow);
                    RaycastHit hit;
                    if(Physics.Raycast(o.transform.position,-Vector3.up,out hit,Distance)){ //使用射线检测距离
                        if(!control.RagdollParts.Contains(hit.collider)
                        //    &&!Ledge.IsLedge(hit.collider.gameObject)
                           &&!Ledge.IsLedgeChecker(hit.collider.gameObject)
                           &&!Ledge.IsCharacter(hit.collider.gameObject)){
                            control.animationProgress.Ground=hit.collider.transform.root.gameObject;
                            control.animationProgress.LandingPosition=new Vector3(0f,hit.point.y,hit.point.z);
                            if (control.manualInput.enabled){
                                TESTING_SPHERE.transform.position = control.animationProgress.LandingPosition;
                            }
                            return true;
                        }
                    }
                }
            }
            control.animationProgress.Ground=null;
            return false;
        }
    }
}
