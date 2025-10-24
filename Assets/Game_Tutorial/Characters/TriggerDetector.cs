using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    // public enum GeneralBodyPart{
    //     Upper,
    //     Lower,
    //     Arm,
    //     Leg,
    // }
    public class TriggerDetector : MonoBehaviour
    {
        // public GeneralBodyPart generalBodyPart;
        public CharacterControl control;
        // public List<Collider> CollidingParts=new List<Collider>();

        public Vector3 LastPosition;
        public Quaternion LastRotation;
        private void Awake(){
            control=this.GetComponentInParent<CharacterControl>();
        }
        private void OnTriggerEnter(Collider col){
            CheckCollidingBodyParts(col);
            CheckCollidingWeapons(col);
        }
        private void CheckCollidingBodyParts(Collider col){
            if(control.RagdollParts.Contains(col)){ //不希望是自己的部分触碰
                return;
            }
            CharacterControl attacker=col.transform.root.GetComponent<CharacterControl>();
            if (attacker==null){ //触碰到的只是一个物理对象
                return;
            }
            if(col.gameObject==attacker.gameObject){ //不是盒子碰撞器
                return;
            }
            if(!control.animationProgress.CollidingBodyParts.ContainsKey(this)){
                control.animationProgress.CollidingBodyParts.Add(this, new List<Collider>());
            }
            if(!control.animationProgress.CollidingBodyParts[this].Contains(col)){
                control.animationProgress.CollidingBodyParts[this].Add(col);
            }
            // if(!CollidingParts.Contains(col)){
            //     CollidingParts.Add(col);
            // }
        }

        void CheckCollidingWeapons(Collider col){
            if(col.transform.root.gameObject.GetComponent<Weapon>()==null){
                return;
            }
            if(!control.animationProgress.CollidingWeapons.ContainsKey(this)){
                control.animationProgress.CollidingWeapons.Add(this, new List<Collider>());
            }
            if(!control.animationProgress.CollidingWeapons[this].Contains(col)){
                control.animationProgress.CollidingWeapons[this].Add(col);
            }
        }

        private void OnTriggerExit(Collider col){
            CheckExitingBodyParts(col);
            CheckExitingWeapons(col);
        }
        private void CheckExitingBodyParts(Collider col){
            if(control.animationProgress.CollidingBodyParts.ContainsKey(this)){
                if(control.animationProgress.CollidingBodyParts[this].Contains(col)){
                    control.animationProgress.CollidingBodyParts[this].Remove(col);
                }
                if(control.animationProgress.CollidingBodyParts[this].Count==0){
                    control.animationProgress.CollidingBodyParts.Remove(this);
                }
            }
        } 
        void CheckExitingWeapons(Collider col){
             if(control.animationProgress.CollidingWeapons.ContainsKey(this)){
                if(control.animationProgress.CollidingWeapons[this].Contains(col)){
                    control.animationProgress.CollidingWeapons[this].Remove(col);
                }
                if(control.animationProgress.CollidingWeapons[this].Count==0){
                    control.animationProgress.CollidingWeapons.Remove(this);
                }
            }
        }
    }
}