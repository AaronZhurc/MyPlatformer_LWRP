using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class AnimationProgress : MonoBehaviour
    {
        public Dictionary<StateData,int> CurrentRunningAbilities=new Dictionary<StateData,int>();
        public bool CameraShaken;

        public List<PoolObjectType> PoolObjectList=new List<PoolObjectType>(); 
        public bool RagdollTriggered;
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

        [Header("AirControl")]
        public bool Jumped;
        public float AirMomentum;
        //public bool FrameUpdated;
        public bool CancelPull;
        public Vector3 MaxFallVelocity;
        public bool CanWallJump;
        public bool CheckWallBlock;

        [Header("UpdateBoxCollider")]
        // public bool UpdatingBoxCollider;
        public bool UpdatingSpheres;
        public Vector3 TargetSize;
        public float Size_Speed;
        public Vector3 TargetCenter;
        public float Center_Speed;
        public Vector3 LandingPosition;
        public bool IsLanding;

        [Header("Damage Info")]
        public Attack Attack;
        public CharacterControl Attacker;
        public TriggerDetector DamagedTrigger;
        public GameObject AttackingPart;

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
                    if(UpBlockingObjs.Count > 0) {
                        control.RIGID_BODY.velocity=new Vector3(control.RIGID_BODY.velocity.x,0f,control.RIGID_BODY.velocity.z);
                    }
                }
                else {
                    if(UpBlockingObjs.Count != 0) {
                        UpBlockingObjs.Clear();
                    }
                }
            }
        }

        void CheckFrontBlocking(){
            if(LatestMoveForward.Speed>0){
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
                CheckRaycastCollision(o,this.transform.forward*DirBlock,LatestMoveForward.BlockDistance,FrontBlockingObjs);
            }
        }

        void CheckUpBlocking(){
            foreach(GameObject o in control.collisionSpheres.UpSpheres) {
                CheckRaycastCollision(o,this.transform.up,0.3f,UpBlockingObjs);
            }
        }


        void CheckRaycastCollision(GameObject obj,Vector3 dir,float BlockDistance,Dictionary<GameObject,GameObject> BlockingObjDic){
            /*Self=false;*/
            //Draw debug line
            Debug.DrawRay(obj.transform.position,dir*BlockDistance,Color.yellow);

            //Check collision
            RaycastHit hit;
            if(Physics.Raycast(obj.transform.position,dir,out hit,BlockDistance)){ //使用射线检测距离
                if(!IsBodyPart(hit.collider) //如果不是身体一部分
                    &&!IsIgnoreCharacter(hit.collider)
                    // &&!Ledge.IsLedge(hit.collider.gameObject)
                    &&!Ledge.IsLedgeChecker(hit.collider.gameObject)
                    &&!Weapon.IsWeapon(hit.collider.gameObject)
                    &&!TrapSpikes.IsTrap(hit.collider.gameObject)){ 
                    if(BlockingObjDic.ContainsKey(obj)){
                        BlockingObjDic[obj] = hit.collider.transform.root.gameObject;
                    }else{
                        BlockingObjDic.Add(obj, hit.collider.transform.root.gameObject);
                    }
                }else{
                    if(BlockingObjDic.ContainsKey(obj)){
                        BlockingObjDic.Remove(obj);
                    }
                }
            }else{
                if(BlockingObjDic.ContainsKey(obj)){
                    BlockingObjDic.Remove(obj);
                }
            }
        }

        bool IsIgnoreCharacter(Collider col){
            if(!IsIgnoreCharacterTime){
                return false;
            }else{
                CharacterControl blockingChar=CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);
                if(blockingChar==null){
                    return false;
                }
                if(blockingChar==control){
                    return false;
                }
                else{
                    return true;
                }
            }
        }

        bool IsBodyPart(Collider col){
            // CharacterControl control=col.transform.root.GetComponent<CharacterControl>();
            // if(control==null){ //如果不是身体一部分
            //     return false;
            // }
            // if(control.gameObject==col.gameObject){ //如果collider是角色控件自己，即不是身体部分，而是root
            //     return false;
            // }
            // if(control.RagdollParts.Contains(col)){ //在布娃娃部件列表内，即是自己的部件
            //     return true;
            // }
            // return false;

            if(col.transform.root.gameObject==control.gameObject){ //如果是同一部件
                return true;
            }

            //如果不是，就可能是敌人
            CharacterControl target=CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);
            
            if(target==null){ //无CharacterControl，非身体部位
                return false;
            }

            if(target.damageDetector.IsDead()){ //确认是否是死人
                return true;
            }else{
                return false;
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

            foreach(KeyValuePair<StateData, int> data in CurrentRunningAbilities) {
                if(data.Key.name.Contains(str)) {
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