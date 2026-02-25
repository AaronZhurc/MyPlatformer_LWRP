using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class Ragdoll : SubComponent
    {
        public RagdollData ragdollData;

        void Start() {
            ragdollData=new RagdollData {
                RagdollTriggered=false,
                BodyParts=new List<Collider>(),
                GetBody=GetBodyPart,
                AddForceToDamagePart=AddForceToDamagePart,
            };
            SetupBodyParts();
            subComponentProcessor.ragdollData=ragdollData;
            subComponentProcessor.ComponentsDic.Add(SubComponentType.RAGDOLL,this);
            //control.ProcDic.Add(CharacterProc.RAGDOLL_ON, TurnOnRagdoll);
        }
        public override void OnFixedUpdate() {
            if(ragdollData.RagdollTriggered) {
                ProcRagdoll();
            }
        }

        public override void OnUpdate() {
            throw new System.NotImplementedException();
        }

        // public void TurnOnRagdoll() {
        //     ragdollData.RagdollTriggered = true;
        // }

        public void SetupBodyParts() {
            ragdollData.BodyParts.Clear();

            Collider[] colliders = control.gameObject.GetComponentsInChildren<Collider>();

            foreach(Collider c in colliders) {
                if(c.gameObject.GetComponent<LedgeChecker>() == null && c.gameObject.GetComponent<LedgeCollider>()==null && c.gameObject.GetComponent<OverlapChecker>()==null) {
                    if(c.gameObject != control.gameObject) { //我们不想将外面的盒子collider作为trigger，我们希望其能与物理环境进行交互
                        c.isTrigger = true; //此时collider会穿过其他物理对象，除非我们能够准确知道其他对象何时解除collider
                        ragdollData.BodyParts.Add(c);
                        c.attachedRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                        c.attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

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

        void ProcRagdoll() {
            ragdollData.RagdollTriggered = false;

            if(control.SkinnedMeshAnimator.avatar == null) {
                return;
            }
            //改变层
            Transform[] arr = control.gameObject.GetComponentsInChildren<Transform>();
            foreach(Transform t in arr) {
                t.gameObject.layer = LayerMask.NameToLayer(RB_Layers.DEADBODY.ToString());
            }

            //设置身体部件位置
            foreach(Collider c in ragdollData.BodyParts) {
                TriggerDetector det = c.GetComponent<TriggerDetector>();
                det.LastPosition = c.gameObject.transform.position;
                det.LastRotation = c.gameObject.transform.rotation;
            }

            //关闭animator/avator/etc
            control.RIGID_BODY.useGravity = false; //关闭重力
            control.RIGID_BODY.velocity = Vector3.zero;
            control.gameObject.GetComponent<BoxCollider>().enabled = false; //此时我们关闭盒子collider
            control.SkinnedMeshAnimator.enabled = false;
            control.SkinnedMeshAnimator.avatar = null;


            //关闭legde colloders
            control.LEDGE_GRAB_DATA.LedgeCollidersOff();

            //关闭ai
            if(control.aiController!=null){
                control.aiController.gameObject.SetActive(false);
                control.navMeshObstacle.enabled=false;
            }

            //打开ragdoll
            foreach(Collider c in ragdollData.BodyParts) {
                c.isTrigger = false; //转换为物理对象


                TriggerDetector det = c.GetComponent<TriggerDetector>();
                //c.transform.localPosition = det.LastPosition;
                //c.transform.localRotation = det.LastRotation;

                c.attachedRigidbody.MovePosition(det.LastPosition);
                c.attachedRigidbody.MoveRotation(det.LastRotation);

                c.attachedRigidbody.velocity = Vector3.zero; //对于陷阱，需要在这里关闭速度以不添加力
            }

            AddForceToDamagePart(false);
        }

        Collider GetBodyPart(string name) {
            foreach(Collider c in ragdollData.BodyParts) {
                if(c.name.Contains(name)) {
                    return c;
                }
            }
            return null;
        }

        void AddForceToDamagePart(bool zeroVelocity) {
            //add force
            if(control.DAMAGE_DATA.DamagedTrigger != null) {
                if(zeroVelocity) {
                    foreach(Collider c in ragdollData.BodyParts) {
                        c.attachedRigidbody.velocity = Vector3.zero;
                    }
                }

                control.DAMAGE_DATA.DamagedTrigger.GetComponent<Rigidbody>()
                    .AddForce(control.DAMAGE_DATA.Attacker.transform.forward * control.DAMAGE_DATA.Attack.ForwardForce
                             + control.DAMAGE_DATA.Attacker.transform.right * control.DAMAGE_DATA.Attack.RightForce
                             + control.DAMAGE_DATA.Attacker.transform.up * control.DAMAGE_DATA.Attack.UpForce);
            }
        }
    }
}