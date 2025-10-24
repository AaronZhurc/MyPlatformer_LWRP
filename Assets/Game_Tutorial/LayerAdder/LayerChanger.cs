using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class LayerChanger : MonoBehaviour
    {
        public RB_Layers LayerType;
        public bool ChangeAllChildren;

        public void ChangeLayer(Dictionary<string,int> layerDic){
            if(!ChangeAllChildren){
                Debug.Log(gameObject.name+" changing Layer: "+LayerType.ToString());
                this.gameObject.layer=layerDic[LayerType.ToString()];
            }else{
                Transform[] arr=this.gameObject.GetComponentsInChildren<Transform>();

                foreach(Transform t in arr){
                    Debug.Log(t.gameObject.name+" changing Layer: "+LayerType.ToString());
                    t.gameObject.layer=layerDic[LayerType.ToString()];
                }
            }
        }
    }
}