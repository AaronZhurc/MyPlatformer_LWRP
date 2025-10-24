using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class CollisionSpheres : MonoBehaviour
    {
        public CharacterControl owner;
        public List<GameObject> BottomSpheres=new List<GameObject>();
        public List<GameObject> FrontSpheres=new List<GameObject>();
        public List<GameObject> BackSpheres=new List<GameObject>();
        public List<GameObject> UpSpheres=new List<GameObject>();
        // Start is called before the first frame update
        
        public List<OverlapChecker> FrontOverlapCheckers=new List<OverlapChecker>();
        
        public void SetColliderSpheres(){
            // BoxCollider box=GetComponent<BoxCollider>();
            // float bottom=box.bounds.center.y-box.bounds.extents.y;
            // float top=box.bounds.center.y+box.bounds.extents.y;
            // float front=box.bounds.center.z+box.bounds.extents.z;
            // float back=box.bounds.center.z-box.bounds.extents.z;

            // GameObject bottomFrontHor=CreateEdgeSphere(new Vector3(0f,bottom,front)); //底部检测点
            // GameObject bottomFrontVer=CreateEdgeSphere(new Vector3(0f,bottom+0.05f,front)); //前部检测点，防止接缝问题
            // GameObject bottomBack=CreateEdgeSphere(new Vector3(0f,bottom,back));
            // GameObject topFront=CreateEdgeSphere(new Vector3(0f,top,front));

            // bottomFrontHor.transform.parent=this.transform; //作为子元素
            // bottomFrontVer.transform.parent=this.transform;
            // bottomBack.transform.parent=this.transform;
            // topFront.transform.parent=this.transform;

            // BottomSpheres.Add(bottomFrontHor);
            // BottomSpheres.Add(bottomBack);

            // FrontSpheres.Add(bottomFrontVer);
            // FrontSpheres.Add(topFront);

            // float horSec=(bottomFrontHor.transform.position-bottomBack.transform.position).magnitude/5f; //多弄5个检测点
            // float verSec=(bottomFrontVer.transform.position-topFront.transform.position).magnitude/10f;

            // CreateMiddleSpheres(bottomFrontHor,-this.transform.forward,horSec,4,BottomSpheres);
            // CreateMiddleSpheres(bottomFrontVer,this.transform.up,verSec,9,FrontSpheres);
            
            //bottom
            for(int i=0;i<5;i++){
                GameObject obj=Instantiate(Resources.Load("ColliderEdge",typeof(GameObject)),Vector3.zero,Quaternion.identity) as GameObject;
                BottomSpheres.Add(obj);
                obj.transform.parent=this.transform.Find("Bottom");
            }
            Reposition_BottomSpheres();

            //up
            for(int i=0;i<5;i++){
                GameObject obj=Instantiate(Resources.Load("ColliderEdge",typeof(GameObject)),Vector3.zero,Quaternion.identity) as GameObject;
                UpSpheres.Add(obj);
                obj.transform.parent=this.transform.Find("Up");
            }
            Reposition_UpSpheres();
            
            //front
            for(int i=0;i<10;i++){
                GameObject obj=Instantiate(Resources.Load("ColliderEdge",typeof(GameObject)),Vector3.zero,Quaternion.identity) as GameObject;
                FrontSpheres.Add(obj);
                FrontOverlapCheckers.Add(obj.GetComponent<OverlapChecker>());
                obj.transform.parent=this.transform.Find("Front");
            }
            Reposition_FrontSpheres();

            //back
            for(int i=0;i<10;i++){
                GameObject obj=Instantiate(Resources.Load("ColliderEdge",typeof(GameObject)),Vector3.zero,Quaternion.identity) as GameObject;
                BackSpheres.Add(obj);
                obj.transform.parent=this.transform.Find("Back");
            }
            Reposition_BackSpheres();
        }

        public void Reposition_FrontSpheres(){
            BoxCollider boxCollider=owner.boxCollider;

            float bottom=boxCollider.bounds.center.y-boxCollider.bounds.size.y/2f;
            float top=boxCollider.bounds.center.y+boxCollider.bounds.size.y/2f;
            float front=boxCollider.bounds.center.z+boxCollider.bounds.size.z/2f;
            //float back=boxCollider.bounds.center.z-boxCollider.bounds.extents.z;
        
            FrontSpheres[0].transform.localPosition=new Vector3(0f,bottom+0.05f,front)-this.transform.position;
            FrontSpheres[1].transform.localPosition=new Vector3(0f,top,front)-this.transform.position;

            float interval=(top-bottom+0.05f)/9;
            for(int i=2;i<FrontSpheres.Count;i++){
                FrontSpheres[i].transform.localPosition=new Vector3(0f,bottom+(interval*(i-1)),front)-this.transform.position;
            }
        }   

        public void Reposition_BackSpheres(){
            BoxCollider boxCollider=owner.boxCollider;

            float bottom=boxCollider.bounds.center.y-boxCollider.bounds.size.y/2f;
            float top=boxCollider.bounds.center.y+boxCollider.bounds.size.y/2f;
            float back=boxCollider.bounds.center.z-boxCollider.bounds.size.z/2f;
            //float back=boxCollider.bounds.center.z-boxCollider.bounds.extents.z;
        
            BackSpheres[0].transform.localPosition=new Vector3(0f,bottom+0.05f,back)-this.transform.position;
            BackSpheres[1].transform.localPosition=new Vector3(0f,top,back)-this.transform.position;

            float interval=(top-bottom+0.05f)/9;
            for(int i=2;i<BackSpheres.Count;i++){
                BackSpheres[i].transform.localPosition=new Vector3(0f,bottom+(interval*(i-1)),back)-this.transform.position;
            }
        }   

        public void Reposition_BottomSpheres(){
            BoxCollider boxCollider=owner.boxCollider;

            float bottom=boxCollider.bounds.center.y-boxCollider.bounds.size.y/2f;
            //float top=boxCollider.bounds.center.y+boxCollider.bounds.extents.y;
            float front=boxCollider.bounds.center.z+boxCollider.bounds.size.z/2f;
            float back=boxCollider.bounds.center.z-boxCollider.bounds.size.z/2f;
        
            BottomSpheres[0].transform.localPosition=new Vector3(0f,bottom,back)-this.transform.position;
            BottomSpheres[1].transform.localPosition=new Vector3(0f,bottom,front)-this.transform.position;

            float interval=(front-back)/4;
            for(int i=2;i<BottomSpheres.Count;i++){
                BottomSpheres[i].transform.localPosition=new Vector3(0f,bottom,back+(interval*(i-1)))-this.transform.position;
            }
        }   

        public void Reposition_UpSpheres(){
            BoxCollider boxCollider=owner.boxCollider;

            float top=boxCollider.bounds.center.y+boxCollider.bounds.size.y/2f;
            //float top=boxCollider.bounds.center.y+boxCollider.bounds.extents.y;
            float front=boxCollider.bounds.center.z+boxCollider.bounds.size.z/2f;
            float back=boxCollider.bounds.center.z-boxCollider.bounds.size.z/2f;
        
            UpSpheres[0].transform.localPosition=new Vector3(0f,top,back)-this.transform.position;
            UpSpheres[1].transform.localPosition=new Vector3(0f,top,front)-this.transform.position;

            float interval=(front-back)/4;
            for(int i=2;i<UpSpheres.Count;i++){
                UpSpheres[i].transform.localPosition=new Vector3(0f,top,back+(interval*(i-1)))-this.transform.position;
            }
        }   
    }
}