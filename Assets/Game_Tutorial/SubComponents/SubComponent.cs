using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    public enum SubComponentType {
        NONE,
        MANUAL_INPUT,
        LEDGE_CHECKER,
        RAGDOLL,
        BLOCKING_OBJECTS,
        BOX_COLLIDER_UPDATER,
        VERTICAL_VELOCITY,
        DAMAGE_DETECTOR,
        COLLISION_SPHERES,
    }
    // public enum BoolData {
    //     NONE,
    //     DOUBLETAP_UP,
    //     DOUBLETAP_DOWN,
    //     // GRABBING_LEDGE,
    //     // UPBLOCKINGOBJDIC_EMPTY,
    //     // FRONTBLOCKINGOBJDIC_EMPTY,
    //     // RIGHTSIDE_BLOCKED,
    //     // LEFTSIDE_BLOCKED,
    // }

    // public enum ListData {
    //     FRONTBLOCKING_CHARACTERS,
    //     FRONTBLOCKING_OBJS,
    // }
    // public enum CharacterProc {
    //     NONE,
    //     // LEDGE_COLLIDERS_OFF,
    //     RAGDOLL_ON,
    //     // CLEAR_FRONTBLOCKINGOBJDIC,
    // }
    public abstract class SubComponent : MonoBehaviour
    {
        protected SubComponentProcessor subComponentProcessor;

        public CharacterControl control {
            get {
                return subComponentProcessor.control;
            }
        }
        private void Awake() {
            subComponentProcessor=this.gameObject.GetComponentInParent<SubComponentProcessor>();
        }

        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
    }
}