using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Games_tutorial
{
    public class AnimationProgress : MonoBehaviour
    {
        public Dictionary<StateData,int> CurrentRunningAbilities=new Dictionary<StateData,int>();
        public bool CameraShaken;

        public List<PoolObjectType> PoolObjectList=new List<PoolObjectType>(); 
        
        // public float MaxPressTime;
        public MoveForward LatestMoveForward;
        public MoveUp LatestMoveUp;
        private List<GameObject> FrontSpheresList;
        // private List<GameObject> UpSpheresList;

        [Header("Attack Button")]
        public bool AttackTriggered;
        public bool AttackButtonIsReset;

        [Header("GroundMovement")]
        public bool disallowEarlyTurn;
        public bool LockDirectionNextState;
        // public bool IsLanding;
        public bool IsIgnoreCharacterTime;
        
        private float DirBlock;

        [Header("Collding Objects")]
        public GameObject Ground;
        public Dictionary<TriggerDetector,List<Collider>> CollidingWeapons=new Dictionary<TriggerDetector, List<Collider>>();
        public Dictionary<TriggerDetector,List<Collider>> CollidingBodyParts=new Dictionary<TriggerDetector, List<Collider>>();
        //前端查看字典信息可使用Odin-Inspector an Serializer
        public Dictionary<GameObject,GameObject> FrontBlockingObjs=new Dictionary<GameObject,GameObject>(); //<where ray from, where ray to>
        public Dictionary<GameObject,GameObject> UpBlockingObjs=new Dictionary<GameObject,GameObject>(); //<where ray from, where ray to>
        public Dictionary<GameObject,GameObject> DownBlockingObjs=new Dictionary<GameObject,GameObject>(); 
        public Vector3 CollidingPoint=new Vector3();

        [Header("AirControl")]
        public bool Jumped;
        public float AirMomentum;
        //public bool FrameUpdated;
        public bool CancelPull;
        public Vector3 MaxFallVelocity;
        public bool CanWallJump;
        public bool CheckWallBlock;
        public List<CharacterControl> MarioStompTargets=new List<CharacterControl>();
        
        [Header("UpdateBoxCollider")]
        // public bool UpdatingBoxCollider;
        public bool UpdatingSpheres;
        public Vector3 TargetSize;
        public float Size_Speed;
        public Vector3 TargetCenter;
        public float Center_Speed;
        public Vector3 LandingPosition;
        public bool IsLanding;


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
            if(control.Attack){
                // PressTime+=Time.deltaTime;
                if(AttackButtonIsReset){
                    AttackTriggered=true;
                    AttackButtonIsReset=false;
                }
            }else{
                // PressTime=0f;
                AttackButtonIsReset=true;
                AttackTriggered=false;
            }
            // if(PressTime==0f){
            //     AttackTriggered=false;
            // }else if(PressTime>MaxPressTime){
            //     AttackTriggered=false;
            // }else{
            //     AttackTriggered=true;
            // }

            if(IsRunning(typeof(LockTransition))){
                if(control.animationProgress.LockTransition){
                    control.SkinnedMeshAnimator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.LockTransition],true);
                }else{
                    control.SkinnedMeshAnimator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.LockTransition],false);
                }
            }else{
                control.SkinnedMeshAnimator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.LockTransition],false);
            }
        }

        // private void LateUpdate(){
        //     FrameUpdated=false;
        // }

        private void FixedUpdate()
        {
            if(IsRunning(typeof(MoveForward))){
                CheckFrontBlocking();
            }else{
                if(FrontBlockingObjs.Count!=0){
                    FrontBlockingObjs.Clear();
                }
            }
            //Checking while LedgeGrab
            if(IsRunning(typeof(MoveUp))) {
                if(LatestMoveUp.Speed > 0f) {
                    CheckUpBlocking();
                }
            }
            else {
                //Checking while jump up
                if(control.RIGID_BODY.velocity.y > 0.001f) {
                    CheckUpBlocking();
                    foreach(KeyValuePair<GameObject,GameObject> data in UpBlockingObjs) {
                        CharacterControl c=CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);

                        if(c == null) {
                            NullifyUpVelocity();
                            break;
                        } else {
                            if(control.transform.position.y + control.boxCollider.center.y < c.transform.position.y) {
                                NullifyUpVelocity();
                                break;
                            }
                        }
                    }
                } else {
                    if(UpBlockingObjs.Count != 0) {
                        UpBlockingObjs.Clear();
                    }
                }
            }

            CheckMarioStop();
        }

        void NullifyUpVelocity() {
            control.RIGID_BODY.velocity=new Vector3(control.RIGID_BODY.velocity.x,0f,control.RIGID_BODY.velocity.z);
        }

        public bool IsFacingAttacker() {
            Vector3 vec=control.damageDetector.Attacker.transform.position-control.transform.position;
            if(vec.z < 0f) {
                if(control.IsFacingForward()){
                    return false;
                } else {
                    return true;
                }
            }else if(vec.z > 0f) {
                if(control.IsFacingForward()){
                    return true;
                } else {
                    return false;
                }
            }
            return true;
        }

        bool ForwardIsReversed() {
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

        void CheckFrontBlocking(){
            if(!ForwardIsReversed()){
                FrontSpheresList=control.collisionSpheres.FrontSpheres;
                DirBlock=1f;
                foreach(GameObject s in control.collisionSpheres.BackSpheres){
                    if(FrontBlockingObjs.ContainsKey(s)){
                        FrontBlockingObjs.Remove(s);
                    }
                }
            }else{
                FrontSpheresList=control.collisionSpheres.BackSpheres;
                DirBlock=-1f;
                foreach(GameObject s in control.collisionSpheres.FrontSpheres){
                    if(FrontBlockingObjs.ContainsKey(s)){
                        FrontBlockingObjs.Remove(s);
                    }
                }
            }

            foreach(GameObject o in FrontSpheresList) {
                //CheckRaycastCollision(o,this.transform.forward*DirBlock,LatestMoveForward.BlockDistance,FrontBlockingObjs);
                GameObject blockingObj=CollisionDetection.GetCollidingObject(control,o,this.transform.forward*DirBlock,LatestMoveForward.BlockDistance,ref control.animationProgress.CollidingPoint);
                if(blockingObj != null) {
                   AddBlockingObjToDic(FrontBlockingObjs,o,blockingObj);
                }
                else {
                    RemoveBlockingObjFromDic(FrontBlockingObjs,o);
                }
            }
        }

        void CheckMarioStop() {
            if(control.RIGID_BODY.velocity.y >= 0f) {
                MarioStompTargets.Clear();
                DownBlockingObjs.Clear();
                return;
            }

            if(MarioStompTargets.Count > 0) {
                control.RIGID_BODY.velocity=Vector3.zero;
                control.RIGID_BODY.AddForce(Vector3.up*250f);

                foreach(CharacterControl c in MarioStompTargets) {
                    AttackInfo info=new AttackInfo();
                    info.CopyInfo(control.damageDetector.MarioStampAttack, control);

                    int index=Random.Range(0,c.BodyParts.Count);
                    c.damageDetector.DamagedTrigger=c.BodyParts[index].GetComponent<TriggerDetector>();
                    c.damageDetector.Attack=control.damageDetector.MarioStampAttack;
                    c.damageDetector.Attacker=control;
                    c.damageDetector.AttackingPart=control.RightFoot_Attack;

                    c.damageDetector.TakeDamage(info);
                }

                MarioStompTargets.Clear();
                return;
            }

            CheckDownBlocking();
            if(DownBlockingObjs.Count > 0) {
                foreach(KeyValuePair<GameObject,GameObject> data in DownBlockingObjs) {
                    CharacterControl c=CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);
                    if(c != null) {
                        if(c.boxCollider.center.y + c.transform.position.y < control.transform.position.y) {
                            if(c != control) {
                                if(!MarioStompTargets.Contains(c)) {
                                    MarioStompTargets.Add(c);
                                }                        
                            }
                        }
                    }
                }
            }
        }

        void AddBlockingObjToDic(Dictionary<GameObject,GameObject> dic,GameObject key,GameObject value) {
            if(dic.ContainsKey(key)) {
                dic[key]=value;
            }
            else {
                dic.Add(key,value);
            }
        }

        void RemoveBlockingObjFromDic(Dictionary<GameObject,GameObject> dic,GameObject key) {
            if(dic.ContainsKey(key)) {
                dic.Remove(key);
            }
        }

        void CheckDownBlocking(){
            foreach(GameObject o in control.collisionSpheres.BottomSpheres) {
                //CheckRaycastCollision(o,Vector3.down,0.1f,DownBlockingObjs);
                GameObject blockingObj=CollisionDetection.GetCollidingObject(control,o,Vector3.down,0.1f,ref control.animationProgress.CollidingPoint);
                if(blockingObj != null) {
                   AddBlockingObjToDic(DownBlockingObjs,o,blockingObj);
                }
                else {
                    RemoveBlockingObjFromDic(DownBlockingObjs,o);
                }
            }
        }

        void CheckUpBlocking(){
            foreach(GameObject o in control.collisionSpheres.UpSpheres) {
                //CheckRaycastCollision(o,this.transform.up,0.3f,UpBlockingObjs);
                GameObject blockingObj=CollisionDetection.GetCollidingObject(control,o,this.transform.up,0.1f,ref control.animationProgress.CollidingPoint);
                if(blockingObj != null) {
                   AddBlockingObjToDic(UpBlockingObjs,o,blockingObj);
                }
                else {
                    RemoveBlockingObjFromDic(UpBlockingObjs,o);
                }
            }

        }

        public bool IsRunning(System.Type type) {
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

            foreach(KeyValuePair<StateData,int> data in CurrentRunningAbilities){
                if(data.Key.GetType()==type){
                    return true;
                }
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

        public bool RightSideIsBlocked() {
            foreach(KeyValuePair<GameObject, GameObject> data in FrontBlockingObjs) {
                if((data.Value.transform.position - control.transform.position).z > 0f) {
                    return true;
                }
            }
            return false;
        }

        public bool LeftSideIsBlocked(){
            foreach(KeyValuePair<GameObject,GameObject> data in FrontBlockingObjs){
                if((data.Value.transform.position-control.transform.position).z<0f){
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