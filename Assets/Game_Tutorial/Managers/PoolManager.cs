using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Games_tutorial
{
    public class PoolManager : Singleton<PoolManager>
    {
        public Dictionary<PoolObjectType,List<GameObject>> PoolDictionary=new Dictionary<PoolObjectType, List<GameObject>>(); //键为我们要访问的所用pool对象的表  
        public void SetUpDictionary(){
            PoolObjectType[] arr=System.Enum.GetValues(typeof(PoolObjectType)) as PoolObjectType[]; 

            foreach (PoolObjectType p in arr)
            {
                if(!PoolDictionary.ContainsKey(p)){
                    PoolDictionary.Add(p,new List<GameObject>());
                }
            }
        }

        public GameObject GetObject(PoolObjectType objType){
            if(PoolDictionary.Count==0){
                SetUpDictionary();
            }
            List<GameObject> list=PoolDictionary[objType];
            GameObject obj=null;
            if(list.Count>0){ //如果存在对象，我们获取这个对象，并移出
                obj=list[0];
                list.RemoveAt(0);
            }else{ //如果没有，实例化一个新的预制件
                obj=PoolObjectLoader.InstantiatePrefab(objType).gameObject;
                /*if(obj==null){
                    Debug.Log(objType.ToString());
                }*/
            }
            return obj;
        }

        public void AddObject(PoolObject obj){
            List<GameObject> list=PoolDictionary[obj.poolObjectType];
            list.Add(obj.gameObject);
            obj.gameObject.SetActive(false); //放回后我们要将其关闭
        }
    }
}