using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
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
        public ManualInput manualInput;
        public LedgeChecker ledgeChecker;
        public AnimationProgress animationProgress;
        public AIProgress aiProgress;

        public DamageDetector damageDetector;

        //public GameObject ColliderEdgePrefab;
        public CollisionSpheres collisionSpheres;

        public AIController aiController;

        //public List<Collider> CollidingParts=new List<Collider>(); //存储所有接触的身体部位 //我们希望其位于每个触发器所在的位置

        public BoxCollider boxCollider;
        public NavMeshObstacle navMeshObstacle;

        [Header("Gravity")]
        // public float GravityMultipilier; //坠落时获得动量
        // public float PullMultipilier; //放开跳跃按钮时获得的拉力
        public ContactPoint[] contactPoints;

        [Header("Setup")]  //必须手动设置
        public PlayableCharacterType playableCharacterType;
        public Animator SkinnedMeshAnimator;
        public Material material;
        public List<Collider> RagdollParts = new List<Collider>();
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
            manualInput = GetComponent<ManualInput>();
            ledgeChecker = GetComponentInChildren<LedgeChecker>();
            animationProgress = GetComponent<AnimationProgress>();
            aiProgress = GetComponentInChildren<AIProgress>();
            damageDetector = GetComponentInChildren<DamageDetector>();
            aiController = GetComponentInChildren<AIController>();
            boxCollider = GetComponent<BoxCollider>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();

            // bool SwitchBack=false;
            // if(!IsFacingForward()){
            //     SwitchBack=true;
            // }
            // FaceForward(true);
            //SetRagdollParts(); //一定要在下面的操作之前，因为检测点也是collider

            collisionSpheres = GetComponentInChildren<CollisionSpheres>();
            collisionSpheres.owner = this;
            collisionSpheres.SetColliderSpheres();

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
            contactPoints = collision.contacts;
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


        public void SetRagdollParts() {
            RagdollParts.Clear();

            Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();

            foreach(Collider c in colliders) {
                if(c.gameObject.GetComponent<LedgeChecker>() == null) {
                    if(c.gameObject != this.gameObject) { //我们不想将外面的盒子collider作为trigger，我们希望其能与物理环境进行交互
                        c.isTrigger = true; //此时collider会穿过其他物理对象，除非我们能够准确知道其他对象何时解除collider
                        RagdollParts.Add(c);
                        c.attachedRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                        c.attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                        CharacterJoint joint = c.GetComponent<CharacterJoint>();
                        if(joint != null) {
                            joint.enableProjection = true;
                        }

                        if(c.GetComponent<TriggerDetector>() == null) {
                            c.gameObject.AddComponent<TriggerDetector>(); //我们不太想从层次结构的最顶层检测触发器，因为这也会检测到顶层的盒子碰撞器，我们只想检测身体部位的碰撞器
                        }

                    }
                }
            }
        }

        public void TurnOnRagdoll() {
            //改变层
            Transform[] arr = GetComponentsInChildren<Transform>();
            foreach(Transform t in arr) {
                t.gameObject.layer = LayerMask.NameToLayer(RB_Layers.DEADBODY.ToString());
            }

            //设置身体部件位置
            foreach(Collider c in RagdollParts) {
                TriggerDetector det = c.GetComponent<TriggerDetector>();
                det.LastPosition = c.gameObject.transform.localPosition;
                det.LastRotation = c.gameObject.transform.localRotation;
            }

            //关闭animator/avator/etc
            RIGID_BODY.useGravity = false; //关闭重力
            RIGID_BODY.velocity = Vector3.zero;
            this.gameObject.GetComponent<BoxCollider>().enabled = false; //此时我们关闭盒子collider
            SkinnedMeshAnimator.enabled = false;
            SkinnedMeshAnimator.avatar = null;

            //打开ragdoll
            foreach(Collider c in RagdollParts) {
                c.isTrigger = false; //转换为物理对象


                TriggerDetector det = c.GetComponent<TriggerDetector>();
                c.transform.localPosition = det.LastPosition;
                c.transform.localRotation = det.LastRotation;

                c.attachedRigidbody.velocity = Vector3.zero; //对于陷阱，需要在这里关闭速度以不添加力
            }

            AddForceToDamagePart(false);
        }

        public void AddForceToDamagePart(bool zeroVelocity) {
            //add force
            if(animationProgress.DamagedTrigger != null) {
                if(zeroVelocity) {
                    foreach(Collider c in RagdollParts) {
                        c.attachedRigidbody.velocity = Vector3.zero;
                    }
                }

                animationProgress.DamagedTrigger.GetComponent<Rigidbody>()
                    .AddForce(animationProgress.Attacker.transform.forward * animationProgress.Attack.ForwardForce
                             + animationProgress.Attacker.transform.right * animationProgress.Attack.RightForce
                             + animationProgress.Attacker.transform.up * animationProgress.Attack.UpForce);
            }
        }

        public void UpdateBoxCollider_Size() {
            // if(!animationProgress.UpdatingBoxCollider){
            //     return;
            // }
            if(!animationProgress.IsRunning(typeof(UpdateBoxCollider))) {
                return;
            }
            if(Vector3.SqrMagnitude(boxCollider.size - animationProgress.TargetSize) > 0.00001f) {
                boxCollider.size = Vector3.Lerp(boxCollider.size, animationProgress.TargetSize, Time.deltaTime * animationProgress.Size_Speed);
                animationProgress.UpdatingSpheres = true;
            }
        }

        public void UpdateBoxCollider_Center() {
            // if(!animationProgress.UpdatingBoxCollider){
            //     return;
            // }
            if(!animationProgress.IsRunning(typeof(UpdateBoxCollider))) {
                return;
            }
            if(Vector3.SqrMagnitude(boxCollider.center - animationProgress.TargetCenter) > 0.0001f) {
                boxCollider.center = Vector3.Lerp(boxCollider.center, animationProgress.TargetCenter, Time.deltaTime * animationProgress.Center_Speed);
            }
        }

        private void FixedUpdate() {
            if(!animationProgress.CancelPull) {
                // if(RIGID_BODY.velocity.y<0f){ //向下
                //     RIGID_BODY.velocity+=-Vector3.up*GravityMultipilier;
                // }
                if(RIGID_BODY.velocity.y > 0f && !Jump) {
                    // RIGID_BODY.velocity+=-Vector3.up*PullMultipilier;
                    RIGID_BODY.velocity -= Vector3.up * RIGID_BODY.velocity.y * 0.1f; //可以通过跳跃键摁下时间控制跳跃高度
                }
            }
            animationProgress.UpdatingSpheres = false;
            UpdateBoxCollider_Size();
            UpdateBoxCollider_Center();
            if(animationProgress.UpdatingSpheres) {
                collisionSpheres.Reposition_FrontSpheres();
                collisionSpheres.Reposition_BackSpheres();
                collisionSpheres.Reposition_BottomSpheres();
                collisionSpheres.Reposition_UpSpheres();
                if(animationProgress.IsLanding) {
                    RIGID_BODY.MovePosition(new Vector3(0f, animationProgress.LandingPosition.y, this.transform.position.z));
                    
                }
            }

            if(animationProgress.RagdollTriggered) {
                TurnOnRagdoll();
                animationProgress.RagdollTriggered = false;
            }

            //slow down wallslide
            if(animationProgress.MaxFallVelocity.y != 0f) {
                if(RIGID_BODY.velocity.y <= animationProgress.MaxFallVelocity.y) {
                    RIGID_BODY.velocity = animationProgress.MaxFallVelocity;
                }
            }
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

        public void FaceForward(bool forward) {
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(RBScenes.TutorialScene_CharacterSelect.ToString())) {
                return;
            }

            if(!SkinnedMeshAnimator.enabled) {
                return;
            }

            if(forward) {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        public bool IsFacingForward() {
            return transform.forward.z > 0f;
        }

        public Collider GetBodyPart(string name) {
            foreach(Collider c in RagdollParts) {
                if(c.name.Contains(name)) {
                    return c;
                }
            }
            return null;
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
