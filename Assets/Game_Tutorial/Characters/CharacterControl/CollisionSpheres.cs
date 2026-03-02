using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class CollisionSpheres : SubComponent
    {
        public CollisionSphereData collisionSphereData;
        
        // Start is called before the first frame update
        void Start() {
            collisionSphereData=new CollisionSphereData {
                BottomSpheres=new List<GameObject>(),
                FrontSpheres=new List<GameObject>(),
                BackSpheres=new List<GameObject>(),
                UpSpheres=new List<GameObject>(),
                FrontOverlapCheckers=new List<OverlapChecker>(),
                AllOverlapCheckers=new List<OverlapChecker>(),

                Reposition_BackSpheres=Reposition_BackSpheres,
                Reposition_FrontSpheres=Reposition_FrontSpheres,
                Reposition_BottomSpheres=Reposition_BottomSpheres,
                Reposition_UpSpheres=Reposition_UpSpheres,
            };
            subComponentProcessor.collisionSphereData=collisionSphereData;
            subComponentProcessor.ComponentsDic.Add(SubComponentType.COLLISION_SPHERES,this);

            SetColliderSpheres();
        }

        GameObject LoadCollisionSphere(){
            return Instantiate(Resources.Load("CollisionSphere",typeof(GameObject)),Vector3.zero,Quaternion.identity) as GameObject;
        }

        void SetColliderSpheres(){
            // BoxCollider box=GetComponent<BoxCollider>();
            // float bottom=box.bounds.center.y-box.bounds.extents.y;
            // float top=box.bounds.center.y+box.bounds.extents.y;
            // float front=box.bounds.center.z+box.bounds.extents.z;
            // float back=box.bounds.center.z-box.bounds.extents.z;

            // GameObject bottomFrontHor=CreateEdgeSphere(new Vector3(0f,bottom,front)); //底部检测点
            // GameObject bottomFrontVer=CreateEdgeSphere(new Vector3(0f,bottom+0.05f,front)); //前部检测点，防止接缝问题
            // GameObject bottomBack=CreateEdgeSphere(new Vector3(0f,bottom,back));
            // GameObject topFront=CreateEdgeSphere(new Vector3(0f,top,front));

            // bottomFrontHor.transform.parent=control.transform; //作为子元素
            // bottomFrontVer.transform.parent=control.transform;
            // bottomBack.transform.parent=control.transform;
            // topFront.transform.parent=control.transform;

            // BottomSpheres.Add(bottomFrontHor);
            // BottomSpheres.Add(bottomBack);

            // FrontSpheres.Add(bottomFrontVer);
            // FrontSpheres.Add(topFront);

            // float horSec=(bottomFrontHor.transform.position-bottomBack.transform.position).magnitude/5f; //多弄5个检测点
            // float verSec=(bottomFrontVer.transform.position-topFront.transform.position).magnitude/10f;

            // CreateMiddleSpheres(bottomFrontHor,-control.transform.forward,horSec,4,BottomSpheres);
            // CreateMiddleSpheres(bottomFrontVer,control.transform.up,verSec,9,FrontSpheres);
            
            //bottom
            for(int i=0;i<5;i++){
                GameObject obj=LoadCollisionSphere();
                collisionSphereData.BottomSpheres.Add(obj);
                obj.transform.parent=this.transform.Find("Bottom");
            }
            Reposition_BottomSpheres();

            //up
            for(int i=0;i<5;i++){
                GameObject obj=LoadCollisionSphere();
                collisionSphereData.UpSpheres.Add(obj);
                obj.transform.parent=this.transform.Find("Up");
            }
            Reposition_UpSpheres();
            
            //front
            for(int i=0;i<10;i++){
                GameObject obj=LoadCollisionSphere();
                collisionSphereData.FrontSpheres.Add(obj);
                collisionSphereData.FrontOverlapCheckers.Add(obj.GetComponent<OverlapChecker>());
                obj.transform.parent=this.transform.Find("Front");
            }
            Reposition_FrontSpheres();

            //back
            for(int i=0;i<10;i++){
                GameObject obj=LoadCollisionSphere();
                collisionSphereData.BackSpheres.Add(obj);
                obj.transform.parent=this.transform.Find("Back");
            }
            Reposition_BackSpheres();

            //add everything
            OverlapChecker[] arr=this.gameObject.GetComponentsInChildren<OverlapChecker>();
            collisionSphereData.AllOverlapCheckers.Clear();
            collisionSphereData.AllOverlapCheckers.AddRange(arr);
        }

        void Reposition_FrontSpheres(){
            BoxCollider boxCollider=control.boxCollider;

            float bottom=boxCollider.bounds.center.y-boxCollider.bounds.size.y/2f;
            float top=boxCollider.bounds.center.y+boxCollider.bounds.size.y/2f;
            float front=boxCollider.bounds.center.z+boxCollider.bounds.size.z/2f;
            //float back=boxCollider.bounds.center.z-boxCollider.bounds.extents.z;
        
            collisionSphereData.FrontSpheres[0].transform.localPosition=new Vector3(0f,bottom+0.05f,front)-control.transform.position;
            collisionSphereData.FrontSpheres[1].transform.localPosition=new Vector3(0f,top,front)-control.transform.position;

            float interval=(top-bottom+0.05f)/9;
            for(int i=2;i<collisionSphereData.FrontSpheres.Count;i++){
                collisionSphereData.FrontSpheres[i].transform.localPosition=new Vector3(0f,bottom+(interval*(i-1)),front)-control.transform.position;
            }
        }   

        void Reposition_BackSpheres(){
            BoxCollider boxCollider=control.boxCollider;

            float bottom=boxCollider.bounds.center.y-boxCollider.bounds.size.y/2f;
            float top=boxCollider.bounds.center.y+boxCollider.bounds.size.y/2f;
            float back=boxCollider.bounds.center.z-boxCollider.bounds.size.z/2f;
            //float back=boxCollider.bounds.center.z-boxCollider.bounds.extents.z;
        
            collisionSphereData.BackSpheres[0].transform.localPosition=new Vector3(0f,bottom+0.05f,back)-control.transform.position;
            collisionSphereData.BackSpheres[1].transform.localPosition=new Vector3(0f,top,back)-control.transform.position;

            float interval=(top-bottom+0.05f)/9;
            for(int i=2;i<collisionSphereData.BackSpheres.Count;i++){
                collisionSphereData.BackSpheres[i].transform.localPosition=new Vector3(0f,bottom+(interval*(i-1)),back)-control.transform.position;
            }
        }   

        void Reposition_BottomSpheres(){
            BoxCollider boxCollider=control.boxCollider;

            float bottom=boxCollider.bounds.center.y-boxCollider.bounds.size.y/2f;
            //float top=boxCollider.bounds.center.y+boxCollider.bounds.extents.y;
            float front=boxCollider.bounds.center.z+boxCollider.bounds.size.z/2f;
            float back=boxCollider.bounds.center.z-boxCollider.bounds.size.z/2f;
        
            collisionSphereData.BottomSpheres[0].transform.localPosition=new Vector3(0f,bottom,back)-control.transform.position;
            collisionSphereData.BottomSpheres[1].transform.localPosition=new Vector3(0f,bottom,front)-control.transform.position;

            float interval=(front-back)/4;
            for(int i=2;i<collisionSphereData.BottomSpheres.Count;i++){
                collisionSphereData.BottomSpheres[i].transform.localPosition=new Vector3(0f,bottom,back+(interval*(i-1)))-control.transform.position;
            }
        }   

        void Reposition_UpSpheres(){
            BoxCollider boxCollider=control.boxCollider;

            float top=boxCollider.bounds.center.y+boxCollider.bounds.size.y/2f;
            //float top=boxCollider.bounds.center.y+boxCollider.bounds.extents.y;
            float front=boxCollider.bounds.center.z+boxCollider.bounds.size.z/2f;
            float back=boxCollider.bounds.center.z-boxCollider.bounds.size.z/2f;
        
            collisionSphereData.UpSpheres[0].transform.localPosition=new Vector3(0f,top,back)-control.transform.position;
            collisionSphereData.UpSpheres[1].transform.localPosition=new Vector3(0f,top,front)-control.transform.position;

            float interval=(front-back)/4;
            for(int i=2;i<collisionSphereData.UpSpheres.Count;i++){
                collisionSphereData.UpSpheres[i].transform.localPosition=new Vector3(0f,top,back+(interval*(i-1)))-control.transform.position;
            }
        }

        public override void OnUpdate() {

        }

        public override void OnFixedUpdate() {
            foreach(OverlapChecker checker in collisionSphereData.AllOverlapCheckers) {
                checker.UpdateChecker();
            }
        }
    }
}