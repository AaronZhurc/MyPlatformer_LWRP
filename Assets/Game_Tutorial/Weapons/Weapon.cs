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

        [Header("Weapon Throw")]
        public Vector3 ThrowOffset=new Vector3();
        public bool IsThrown;
        public bool FlyForward;
        public float FlightSpeed;
        public float RotationSpeed;
        public CharacterControl Thrower;
        public GameObject WeaponTip;

        private void Start() {
            IsThrown=false;
        }

        private void FixedUpdate() {
            if(IsThrown) {
                if(FlyForward) {
                    this.transform.position+=Vector3.forward*FlightSpeed*Time.deltaTime;
                }
                else {
                    this.transform.position-=Vector3.forward*FlightSpeed*Time.deltaTime;
                }
                this.transform.Rotate(Vector3.forward, RotationSpeed*Time.deltaTime);
            }
        }

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
                RemoveWeaponFromDictionary(control);
                
                w.transform.position=control.transform.position;

                control.animationProgress.HoldingWeapon=null;
                control=null;
                w.triggerDetector.control=null;
            }
        }

        public void ThrowWeapon() {
            Weapon w=control.animationProgress.HoldingWeapon;
            if(w!=null){
                w.transform.parent=null;
                if(control.IsFacingForward()){
                    w.transform.rotation=Quaternion.Euler(0f,90f,0f);
                }else{
                    w.transform.rotation=Quaternion.Euler(0f,-90f,0f);
                }

                FlyForward=control.IsFacingForward();
                
                w.transform.position=control.transform.position+(Vector3.up*ThrowOffset.y);
                w.transform.position+=control.transform.forward*ThrowOffset.z;

                Thrower=control;
                control.animationProgress.HoldingWeapon=null;
                control=null;
                w.triggerDetector.control=null;

                IsThrown=true;

                RemoveWeaponFromDictionary(Thrower);
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

        public void RemoveWeaponFromDictionary(CharacterControl c) {
            foreach (Collider col in c.RAGDOLL_DATA.BodyParts)
            {
                TriggerDetector t=col.GetComponent<TriggerDetector>();
                
                if(t!=null){
                    ProcRemove(c.animationProgress.CollidingWeapons,t);
                    ProcRemove(c.animationProgress.CollidingBodyParts,t);
                }          
            }
        }

        public void ProcRemove(Dictionary<TriggerDetector,List<Collider>> d,TriggerDetector t) {
            if(d.ContainsKey(t)) {
                if(d[t].Contains(PickUpCollider)) {
                    d[t].Remove(PickUpCollider);
                }
                if(d[t].Contains(AttackCollider)) {
                    d[t].Remove(AttackCollider);
                }

                if(d[t].Count == 0) {
                    d.Remove(t);
                }
            }
        }

    }
}