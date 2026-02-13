using System.Collections;
using System.Collections.Generic;
using Games_tutorial.Datasets;
using UnityEngine;


namespace Games_tutorial
{
    public class OverlapChecker : MonoBehaviour
    {
        CharacterControl control;
        public Collider[] arrColliders;
        public bool ObjIsOverlapping;

        void Start()
        {
            control=this.transform.root.gameObject.GetComponent<CharacterControl>();
        }
        private void FixedUpdate()
        {
            if(control.JUMP_DATA.CheckWallBlock){
                if(control.collisionSpheres.FrontOverlapCheckers.Contains(this)){
                    ObjIsOverlapping=CheckObj();
                }
            }else{
                ObjIsOverlapping=false;
            }
        }

        private bool CheckObj(){
            arrColliders=Physics.OverlapSphere(this.transform.position,0.13f);

            foreach(Collider c in arrColliders){
                if(CharacterManager.Instance.GetCharacter(c.transform.root.gameObject)==null){
                    return true;
                }
            }
            return false;
        }
    }
}