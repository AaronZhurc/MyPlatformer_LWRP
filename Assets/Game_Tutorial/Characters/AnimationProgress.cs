using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Games_tutorial
{
    public class AnimationProgress : MonoBehaviour
    {
        
        public bool CameraShaken;

        public List<PoolObjectType> PoolObjectList=new List<PoolObjectType>(); 
        
        // public float MaxPressTime;
        public MoveForward LatestMoveForward;
        public MoveUp LatestMoveUp;


       

        [Header("GroundMovement")]
        // public bool IsLanding;
        public bool IsIgnoreCharacterTime;
        
        

        [Header("Collding Objects")]
        public Dictionary<TriggerDetector,List<Collider>> CollidingWeapons=new Dictionary<TriggerDetector, List<Collider>>();
        public Dictionary<TriggerDetector,List<Collider>> CollidingBodyParts=new Dictionary<TriggerDetector, List<Collider>>();
        

        // [Header("AirControl")]
        // public bool Jumped;
        // public float AirMomentum;
        // //public bool FrameUpdated;
        // public bool CancelPull;
        // public Vector3 MaxFallVelocity;
        // public bool CanWallJump;
        // public bool CheckWallBlock;

        [Header("Transition")]
        public bool LockTransition;

        [Header("Weapon")]
        public Weapon HoldingWeapon;

        private CharacterControl control;
        // private float PressTime; //使用[SerializeField]可在unity中显示该数字

        private void Awake(){
            control = GetComponentInParent<CharacterControl>();
            //PressTime=0f;
        }
        private void Update(){
            
        }

        // private void LateUpdate(){
        //     FrameUpdated=false;
        // }

        private void FixedUpdate()
        {
            
        }

        public void NullifyUpVelocity() {
            control.RIGID_BODY.velocity=new Vector3(control.RIGID_BODY.velocity.x,0f,control.RIGID_BODY.velocity.z);
        }

        public bool IsFacingAttacker() {
            Vector3 vec=control.DAMAGE_DATA.Attacker.transform.position-control.transform.position;
            if(vec.z < 0f) {
                if(control.ROTATION_DATA.IsFacingForward()){
                    return false;
                } else {
                    return true;
                }
            }else if(vec.z > 0f) {
                if(control.ROTATION_DATA.IsFacingForward()){
                    return true;
                } else {
                    return false;
                }
            }
            return true;
        }

        public bool ForwardIsReversed() {
            if(LatestMoveForward.MoveOnHit){
                if(IsFacingAttacker()) {
                    return true;
                }
                else {
                    return false;
                }
            }
            if(LatestMoveForward.Speed > 0f) {
                return false;
            } else if(LatestMoveForward.Speed < 0f) {
                return true;
            }
            return false;
        }


        
        
        public bool StateNameContains(string str) {
            // for(int i=0;i<CurrentRunningAbilities.Count;i++){
            //     if(type==CurrentRunningAbilities[i].GetType()){
            //         if(CurrentRunningAbilities[i]==self){
            //             return false;
            //         }else{
            //             //Debug.Log(type.ToString()+" is already running");
            //             return true;
            //         }
            //     }
            // }
            // return false;

            // foreach(KeyValuePair<StateData, int> data in CurrentRunningAbilities) {
            //     if(data.Key.name.Contains(str)) {
            //         return true;
            //     }
            // }
            // return false;

            AnimatorClipInfo[] arr=control.SkinnedMeshAnimator.GetCurrentAnimatorClipInfo(0);

            foreach(AnimatorClipInfo clipInfo in arr) {
                if(clipInfo.clip.name.Contains(str)) {
                    return true;
                }
            }
            return false;
        }

        

        public Weapon GetTouchingWeapon(){
            foreach(KeyValuePair<TriggerDetector,List<Collider>> data in CollidingWeapons){
                Weapon w=data.Value[0].gameObject.GetComponent<Weapon>();
                return w;
            }
            return null;
        }
    }
}