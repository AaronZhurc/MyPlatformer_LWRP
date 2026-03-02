using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using Games_tutorial.Datasets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace Games_tutorial {
    public enum TransitionParameter {
        Move,
        Jump,
        ForceTransition,
        Grounded,
        Attack,
        ClickAnimation,
        TransitionIndex,
        Turbo,
        Turn,
        LockTransition,
    }
    public enum RBScenes {
        TutorialScene_Sample,
        TutorialScene_CharacterSelect,
    }
    public class CharacterControl:MonoBehaviour {
        [Header("Input")]  //和输入相关
        public bool Turbo;
        public bool MoveUp;
        public bool MoveDown;
        public bool MoveRight;
        public bool MoveLeft;
        public bool Jump;
        public bool Attack;
        public bool Block;

        [Header("SubComponents")]
        public SubComponentProcessor subComponentProcessor;
        //public ManualInput manualInput;
        //public LedgeChecker ledgeChecker;
        public AnimationProgress animationProgress;
        public AIProgress aiProgress;
        // public DamageDetector damageDetector;
        //public GameObject ColliderEdgePrefab;
        // public CollisionSpheres collisionSpheres;
        public AIController aiController;
        //public List<Collider> CollidingParts=new List<Collider>(); //存储所有接触的身体部位 //我们希望其位于每个触发器所在的位置
        public BoxCollider boxCollider;
        public NavMeshObstacle navMeshObstacle;
        // public InstaKill instaKill;
        

        public DataProcessor dataProcessor;

        public BlockingObjData BLOCKING_DATA => subComponentProcessor.blockingData;
        public LedgeGrabData LEDGE_GRAB_DATA => subComponentProcessor.ledgeGrabData;
        public RagdollData RAGDOLL_DATA => subComponentProcessor.ragdollData;
        public ManualInputData MANUAL_INPUT_DATA => subComponentProcessor.manualInputData;
        public BoxColliderData BOX_COLLIDER_DATA => subComponentProcessor.boxColliderData;
        public DamageData DAMAGE_DATA => subComponentProcessor.damageData;
        public MomentumData MOMENTUM_DATA => subComponentProcessor.momentumData;
        public RotationData ROTATION_DATA => subComponentProcessor.rotationData;
        public JumpData JUMP_DATA => subComponentProcessor.jumpData;
        public CollisionSphereData COLLISION_SPHERE_DATA => subComponentProcessor.collisionSphereData;
        public InstaKillData INSTA_KILL_DATA => subComponentProcessor.instaKillData;
        public GroundData GROUND_DATA => subComponentProcessor.groundData;
        public AttackData ATTACK_DATA => subComponentProcessor.attackData;
        public AnimationData ANIMATION_DATA => subComponentProcessor.animationData;

        public Dataset AIR_CONTROL => dataProcessor.GetDataset(typeof(AirControl));
        

        // public Dictionary<BoolData,GetBool> BoolDic=new Dictionary<BoolData,GetBool>();
        // public delegate bool GetBool();

        // public Dictionary<ListData,GetList> ListDic =new Dictionary<ListData, GetList>();
        // public delegate List<GameObject> GetList();

        //public Dictionary<CharacterProc,CharacterProcDel> ProcDic=new Dictionary<CharacterProc,CharacterProcDel>();
        //public delegate void CharacterProcDel();

        // [Header("Gravity")]
        // public float GravityMultipilier; //坠落时获得动量
        // public float PullMultipilier; //放开跳跃按钮时获得的拉力


        [Header("Setup")]  //必须手动设置
        public PlayableCharacterType playableCharacterType;
        public Animator SkinnedMeshAnimator;
        public Material material;
        
        public GameObject LeftHand_Attack;
        public GameObject RightHand_Attack;
        public GameObject LeftFoot_Attack;
        public GameObject RightFoot_Attack;

        // private List<TriggerDetector> TriggerDetectors=new List<TriggerDetector>();
        private Dictionary<string, GameObject> ChildObjects = new Dictionary<string, GameObject>();


        private Rigidbody rigid;
        public Rigidbody RIGID_BODY {
            get {
                if(rigid == null) {
                    rigid = GetComponent<Rigidbody>();
                }
                return rigid;
            }
        }

        private void Awake() {
            subComponentProcessor=GetComponentInChildren<SubComponentProcessor>();
            //manualInput = GetComponent<ManualInput>();
            //ledgeChecker = GetComponentInChildren<LedgeChecker>();
            animationProgress = GetComponent<AnimationProgress>();
            aiProgress = GetComponentInChildren<AIProgress>();
            // damageDetector = GetComponentInChildren<DamageDetector>();
            aiController = GetComponentInChildren<AIController>();
            boxCollider = GetComponent<BoxCollider>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            // instaKill = GetComponentInChildren<InstaKill>();

            // bool SwitchBack=false;
            // if(!IsFacingForward()){
            //     SwitchBack=true;
            // }
            // FaceForward(true);
            //SetRagdollParts(); //一定要在下面的操作之前，因为检测点也是collider

            // collisionSpheres = GetComponentInChildren<CollisionSpheres>();
            // collisionSpheres.control = this;
            

            dataProcessor=this.gameObject.GetComponentInChildren<DataProcessor>();
            System.Type[] arr={typeof(AirControl),typeof(SomeDataset)};
            // dataProcessor.InitializeSets(arr);

            // if(SwitchBack){
            //     FaceForward(false);
            // }
            if(aiController == null) {
                if(navMeshObstacle != null) {
                    navMeshObstacle.carving = true;
                }
            }

            RegisterCharacter();
            // CacheCharacterControl(SkinnedMeshAnimator);
        }

        public void CacheCharacterControl(Animator animator) {
            CharacterState[] arr = animator.GetBehaviours<CharacterState>();

            foreach(CharacterState c in arr) {
                c.characterControl = this;
            }
        }

        private void OnCollisionStay(Collision collision) {
            GROUND_DATA.BoxColliderContacts = collision.contacts;
        }

        // public List<TriggerDetector> GetAllTrigers(){
        //     if(TriggerDetectors.Count==0){
        //         TriggerDetector[] arr=this.gameObject.GetComponentsInChildren<TriggerDetector>();
        //         foreach (TriggerDetector d in arr)
        //         {
        //             TriggerDetectors.Add(d);
        //         }
        //     }
        //     return TriggerDetectors;
        // }

        private void RegisterCharacter() {
            if(!CharacterManager.Instance.Characters.Contains(this)) {
                CharacterManager.Instance.Characters.Add(this);
            }
        }

        /*private IEnumerator Start(){ //临时代码，只是为了测试布娃娃效果，我们实际上需要一个攻击系统
            yield return new WaitForSeconds(5f); //等待5秒
            RIGID_BODY.AddForce(200f*Vector3.up); //将玩家发射到空中一点点，我们不希望碰撞体在打开时接触地面
            yield return new WaitForSeconds(0.5f);
            TurnOnRagdoll();
        }*/

        private void Update() {
            subComponentProcessor.UpdateSubComponents();
        }

        private void FixedUpdate() {
            subComponentProcessor.FixedUpdateSubComponents();
        }

        // public void CreateMiddleSpheres(GameObject start,Vector3 dir,float sec, int interations, List<GameObject> spheresList){
        //     for (int i=0;i<interations;i++){
        //         Vector3 pos=start.transform.position+(dir*sec*(i+1));
        //         GameObject newObj=CreateEdgeSphere(pos);
        //         newObj.transform.parent=this.transform;
        //         spheresList.Add(newObj);
        //     }
        // }

        // GameObject CreateEdgeSphere(Vector3 pos){
        //     //GameObject obj=Instantiate(ColliderEdgePrefab,pos,Quaternion.identity); //旋转为0-
        //     GameObject obj=Instantiate(Resources.Load("ColliderEdge",typeof(GameObject)),pos,Quaternion.identity) as GameObject;
        //     return obj;
        // }

        public void MoveForward(float Speed, float SpeedGraph) {
            transform.Translate(Vector3.forward * Speed * SpeedGraph * Time.deltaTime);
        }

        public GameObject GetChildObj(string name) {
            if(ChildObjects.ContainsKey(name)) {
                return ChildObjects[name];
            }
            Transform[] arr = this.gameObject.GetComponentsInChildren<Transform>();
            foreach(Transform t in arr) {
                if(t.gameObject.name.Equals(name)) {
                    ChildObjects.Add(name, t.gameObject);
                    return t.gameObject;
                }
            }
            return null;
        }


        public GameObject GetAttackingPart(AttackPartType attackPartType) {
            if(attackPartType == AttackPartType.LEFT_HAND) {
                return LeftHand_Attack;
            }
            else if(attackPartType == AttackPartType.RIGHT_HAND) {
                return RightHand_Attack;
            }
            else if(attackPartType == AttackPartType.LEFT_FOOT) {
                return LeftFoot_Attack;
            }
            else if(attackPartType == AttackPartType.RIGHT_FOOT) {
                return RightFoot_Attack;
            }
            else if(attackPartType == AttackPartType.MELEE_WEAPON) {
                return animationProgress.HoldingWeapon.triggerDetector.gameObject;
            }
            return null;
        }

        // Update is called once per frame
        /*void Update()
        {
            
            if(VirtualInputManager.Instance.MoveRight&&VirtualInputManager.Instance.MoveLeft){
                animator.SetBool(TransitionParameter.Move.ToString(),false);
                return;
            }
             if(!VirtualInputManager.Instance.MoveRight&&!VirtualInputManager.Instance.MoveLeft){
                animator.SetBool(TransitionParameter.Move.ToString(),false);
                return;
            }
            if (VirtualInputManager.Instance.MoveRight){
                this.gameObject.transform.Translate(Vector3.forward*Speed*Time.deltaTime);
                this.gameObject.transform.rotation=Quaternion.Euler(0f,0f,0f);
                animator.SetBool(TransitionParameter.Move.ToString(),true);
            }
            if (VirtualInputManager.Instance.MoveLeft){
                this.gameObject.transform.Translate(Vector3.forward*Speed*Time.deltaTime); //注意此时角色已经旋转，所以不需要负号
                this.gameObject.transform.rotation=Quaternion.Euler(0f,180f,0f);
                animator.SetBool(TransitionParameter.Move.ToString(),true);
            }
            
        }*/
        public void ChangeMaterial() {
            if(material == null) {
                Debug.LogError("No material specified");
            }
            Renderer[] arrMaterials = this.gameObject.GetComponentsInChildren<Renderer>(); //获取每个子部分
            foreach(Renderer r in arrMaterials) {
                if(r.gameObject != this.gameObject) { //我们不想更改角色控件的材质
                    r.material = material;
                }
            }
        }

    }
}
