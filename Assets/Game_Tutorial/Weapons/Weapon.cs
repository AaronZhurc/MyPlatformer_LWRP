using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class Weapon : MonoBehaviour
    {
        public CharacterControl control;
        public BoxCollider PickUpCollider;
        public BoxCollider AttackCollider;
        public TriggerDetector triggerDetector;
        public Vector3 CustomPosition=new Vector3();
        public Vector3 CustomRotation=new Vector3();
        public static bool IsWeapon(GameObject obj){
            if(obj.transform.root.gameObject.GetComponent<Weapon>() != null){
                return true;
            }else{
                return false;
            }
        }

        public void DropWeapon(){
            Weapon w=control.animationProgress.HoldingWeapon;
            if(w!=null){
                w.transform.parent=null;
                if(control.IsFacingForward()){
                    w.transform.rotation=Quaternion.Euler(90f,0f,0f);
                }else{
                    w.transform.rotation=Quaternion.Euler(-90f,0f,0f);
                }
                
                w.transform.position=control.transform.position;

                control.animationProgress.HoldingWeapon=null;
                control=null;
                w.triggerDetector.control=null;
            }
        }

        private void Update(){
            if(control!=null){
                PickUpCollider.enabled=false;
                AttackCollider.enabled=true;
            }else{
                PickUpCollider.enabled=true;
                AttackCollider.enabled=false;
            }
        }
    }
}