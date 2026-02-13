using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class SubComponentProcessor : MonoBehaviour
    {
        public Dictionary<SubComponentType,SubComponent> ComponentsDic=new Dictionary<SubComponentType,SubComponent>();
        public CharacterControl control;

        public BlockingObjData blockingData;
        public LedgeGrabData ledgeGrabData;
        public RagdollData ragdollData;
        public ManualInputData manualInputData;
        public BoxColliderData boxColliderData;
        public VerticalVelocityData verticalVelocityData;
        public DamageData damageData;
        public MomentumData momentumData;
        public RotationData rotationData;
        public JumpData jumpData;

        void Awake() {
            control=GetComponentInParent<CharacterControl>();
        }
        public void FixedUpdateSubComponents() {
            FixedUpdateSubComponent(SubComponentType.LEDGE_CHECKER);
            FixedUpdateSubComponent(SubComponentType.RAGDOLL);
            FixedUpdateSubComponent(SubComponentType.BLOCKING_OBJECTS);
            FixedUpdateSubComponent(SubComponentType.BOX_COLLIDER_UPDATER);
            FixedUpdateSubComponent(SubComponentType.VERTICAL_VELOCITY);
        }

        public void UpdateSubComponents() {
            UpdateSubComponent(SubComponentType.MANUAL_INPUT);
            UpdateSubComponent(SubComponentType.DAMAGE_DETECTOR);
        }

        void UpdateSubComponent(SubComponentType type) {
            if(ComponentsDic.ContainsKey(type)) {
                ComponentsDic[type].OnUpdate();
            }
        }

        void FixedUpdateSubComponent(SubComponentType type) {
            if(ComponentsDic.ContainsKey(type)) {
                ComponentsDic[type].OnFixedUpdate();
            }
        }
    }
}