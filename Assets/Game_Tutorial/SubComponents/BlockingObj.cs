using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;
using UnityEngine;

namespace Games_tutorial
{
    public class BlockingObj:SubComponent {
        public BlockingObjData blockingData;

        //前端查看字典信息可使用Odin-Inspector an Serializer
        Dictionary<GameObject,GameObject> FrontBlockingObjs=new Dictionary<GameObject,GameObject>(); //<where ray from, where ray to>
        Dictionary<GameObject,GameObject> UpBlockingObjs=new Dictionary<GameObject,GameObject>(); //<where ray from, where ray to>
        Dictionary<GameObject,GameObject> DownBlockingObjs=new Dictionary<GameObject,GameObject>(); 
        List<CharacterControl> MarioStompTargets=new List<CharacterControl>();
        List<GameObject> FrontBlockingObjsList=new List<GameObject>(); //只是获得整个列表，而不映射到碰撞检测器

        List<GameObject> FrontBlockingCharacters=new List<GameObject>();

        List<GameObject> FrontSpheresList;
        // private List<GameObject> UpSpheresList;

        private float DirBlock;

        private void Start() {
            blockingData = new BlockingObjData {
                FrontBlockingDicCount=0,
                UpBlockingDicCount=0,
                ClearFrontBlockingObjDic=ClearFrontBlockingObjsDic,
                LeftSideBlocked=LeftSideIsBlocked,
                RightSideBlocked=RightSideIsBlocked,
                GetFrontBlockingCharacterList=GetFrontBlockingCharacterList,
                GetFrontBlockingObjList=GetFrontBlockingObjList, 
            };

            subComponentProcessor.blockingData=blockingData;

            subComponentProcessor.ComponentsDic.Add(SubComponentType.BLOCKING_OBJECTS,this);

            // control.ProcDic.Add(CharacterProc.CLEAR_FRONTBLOCKINGOBJDIC,ClearFrontBlockingObjsDic);

            // control.BoolDic.Add(BoolData.UPBLOCKINGOBJDIC_EMPTY,UpBlockingObjsDicIsEmpty);
            // control.BoolDic.Add(BoolData.LEFTSIDE_BLOCKED,LeftSideIsBlocked);
            // control.BoolDic.Add(BoolData.RIGHTSIDE_BLOCKED,RightSideIsBlocked);
            // control.BoolDic.Add(BoolData.FRONTBLOCKINGOBJDIC_EMPTY,FrontBlockingObjsDicIsEmpty);

            // control.ListDic.Add(ListData.FRONTBLOCKING_CHARACTERS,GetFrontBlockingCharacters);
            // control.ListDic.Add(ListData.FRONTBLOCKING_OBJS,GetFrontBlockingObjsList);
        }

        public override void OnFixedUpdate() {
            if(control.ANIMATION_DATA.IsRunning(typeof(MoveForward))){
                CheckFrontBlocking();
            }else{
                if(FrontBlockingObjs.Count!=0){
                    FrontBlockingObjs.Clear();
                }
            }
            //Checking while LedgeGrab
            if(control.ANIMATION_DATA.IsRunning(typeof(MoveUp))) {
                if(control.animationProgress.LatestMoveUp.Speed > 0f) {
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
                            control.animationProgress.NullifyUpVelocity();
                            break;
                        } else {
                            if(control.transform.position.y + control.boxCollider.center.y < c.transform.position.y) {
                                control.animationProgress.NullifyUpVelocity();
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
            blockingData.FrontBlockingDicCount=FrontBlockingObjs.Count;
            blockingData.UpBlockingDicCount=UpBlockingObjs.Count;
        }

        

        public override void OnUpdate() {
            
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
                    AttackCondition info=new AttackCondition();
                    info.CopyInfo(control.DAMAGE_DATA.MarioStampAttack, control);

                    int index=Random.Range(0,c.RAGDOLL_DATA.BodyParts.Count);
                    TriggerDetector randomPart=c.RAGDOLL_DATA.BodyParts[index].GetComponent<TriggerDetector>();
                    // c.DAMAGE_DATA.Attack=control.damageDetector.MarioStampAttack;
                    // c.DAMAGE_DATA.Attacker=control;
                    // c.DAMAGE_DATA.AttackingPart=control.RightFoot_Attack;

                    c.DAMAGE_DATA.SetData(control,control.DAMAGE_DATA.MarioStampAttack,randomPart,control.RightFoot_Attack);

                    c.DAMAGE_DATA.TakeDamage(info);
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

        void CheckDownBlocking(){
            foreach(GameObject o in control.COLLISION_SPHERE_DATA.BottomSpheres) {
                //CheckRaycastCollision(o,Vector3.down,0.1f,DownBlockingObjs);
                GameObject blockingObj=CollisionDetection.GetCollidingObject(control,o,Vector3.down,0.1f,ref control.BLOCKING_DATA.RaycastContact);
                if(blockingObj != null) {
                   AddBlockingObjToDic(DownBlockingObjs,o,blockingObj);
                }
                else {
                    RemoveBlockingObjFromDic(DownBlockingObjs,o);
                }
            }
        }

        void CheckUpBlocking(){
            foreach(GameObject o in control.COLLISION_SPHERE_DATA.UpSpheres) {
                //CheckRaycastCollision(o,this.transform.up,0.3f,UpBlockingObjs);
                GameObject blockingObj=CollisionDetection.GetCollidingObject(control,o,this.transform.up,0.1f,ref control.BLOCKING_DATA.RaycastContact);
                if(blockingObj != null) {
                   AddBlockingObjToDic(UpBlockingObjs,o,blockingObj);
                }
                else {
                    RemoveBlockingObjFromDic(UpBlockingObjs,o);
                }
            }

        }

        void CheckFrontBlocking(){
            if(!control.animationProgress.ForwardIsReversed()){
                FrontSpheresList=control.COLLISION_SPHERE_DATA.FrontSpheres;
                DirBlock=1f;
                foreach(GameObject s in control.COLLISION_SPHERE_DATA.BackSpheres){
                    if(FrontBlockingObjs.ContainsKey(s)){
                        FrontBlockingObjs.Remove(s);
                    }
                }
            }else{
                FrontSpheresList=control.COLLISION_SPHERE_DATA.BackSpheres;
                DirBlock=-1f;
                foreach(GameObject s in control.COLLISION_SPHERE_DATA.FrontSpheres){
                    if(FrontBlockingObjs.ContainsKey(s)){
                        FrontBlockingObjs.Remove(s);
                    }
                }
            }

            foreach(GameObject o in FrontSpheresList) {
                //CheckRaycastCollision(o,this.transform.forward*DirBlock,LatestMoveForward.BlockDistance,FrontBlockingObjs);
                GameObject blockingObj=CollisionDetection.GetCollidingObject(control,o,this.transform.forward*DirBlock,control.animationProgress.LatestMoveForward.BlockDistance,ref control.BLOCKING_DATA.RaycastContact);
                if(blockingObj != null) {
                   AddBlockingObjToDic(FrontBlockingObjs,o,blockingObj);
                }
                else {
                    RemoveBlockingObjFromDic(FrontBlockingObjs,o);
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

        void ClearFrontBlockingObjsDic() {
            FrontBlockingObjs.Clear();
        }

        bool UpBlockingObjsDicIsEmpty() {
            if(UpBlockingObjs.Count == 0) {
                return true;
            }
            return false;
        }

        bool FrontBlockingObjsDicIsEmpty() {
            if(FrontBlockingObjs.Count == 0) {
                return true;
            }
            return false;
        }

        List<GameObject> GetFrontBlockingCharacterList() {
            FrontBlockingCharacters.Clear();
            foreach (KeyValuePair<GameObject,GameObject> data in FrontBlockingObjs)
            {
                CharacterControl c = CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);

                if(c != null) {
                    if(!FrontBlockingCharacters.Contains(c.gameObject)) {
                        FrontBlockingCharacters.Add(c.gameObject);
                    }
                }
            }
            return FrontBlockingCharacters;
        }

         List<GameObject> GetFrontBlockingObjList() {
            FrontBlockingObjsList.Clear();
            foreach (KeyValuePair<GameObject,GameObject> data in FrontBlockingObjs)
            {
                if(!FrontBlockingObjsList.Contains(data.Value)) {
                    FrontBlockingObjsList.Add(data.Value);
                }
            }
            return FrontBlockingObjsList;
        }
    }
}