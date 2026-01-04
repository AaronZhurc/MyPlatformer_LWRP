using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class Ragdoll : SubComponent
    {
        public bool RagdollTriggered=false;

        void Start() {
            control.SubComponentsDic.Add(SubComponents.RAGDOLL,this);
            control.ProcDic.Add(CharacterProc.RAGDOLL_ON, TurnOnRagdoll);
        }
        public override void OnFixedUpdate() {
            if(RagdollTriggered) {
                ProcRagdoll();
            }
        }

        public override void OnUpdate() {
            throw new System.NotImplementedException();
        }

        public void TurnOnRagdoll() {
            RagdollTriggered = true;
        }

        void ProcRagdoll() {
            RagdollTriggered = false;

            if(control.SkinnedMeshAnimator.avatar == null) {
                return;
            }
            //改变层
            Transform[] arr = control.gameObject.GetComponentsInChildren<Transform>();
            foreach(Transform t in arr) {
                t.gameObject.layer = LayerMask.NameToLayer(RB_Layers.DEADBODY.ToString());
            }

            //设置身体部件位置
            foreach(Collider c in control.BodyParts) {
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
            control.ProcDic[CharacterProc.LEDGE_COLLIDERS_OFF]();

            //关闭ai
            if(control.aiController!=null){
                control.aiController.gameObject.SetActive(false);
                control.navMeshObstacle.enabled=false;
            }

            //打开ragdoll
            foreach(Collider c in control.BodyParts) {
                c.isTrigger = false; //转换为物理对象


                TriggerDetector det = c.GetComponent<TriggerDetector>();
                //c.transform.localPosition = det.LastPosition;
                //c.transform.localRotation = det.LastRotation;

                c.attachedRigidbody.MovePosition(det.LastPosition);
                c.attachedRigidbody.MoveRotation(det.LastRotation);

                c.attachedRigidbody.velocity = Vector3.zero; //对于陷阱，需要在这里关闭速度以不添加力
            }

            control.AddForceToDamagePart(false);
        }
    }
}