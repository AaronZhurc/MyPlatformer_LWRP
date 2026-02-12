using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class CollisionDetection : MonoBehaviour
    {
        public static GameObject GetCollidingObject(CharacterControl control,GameObject start,Vector3 dir,float BlockDistance, ref Vector3 collisionPoint){
            collisionPoint=Vector3.zero;

            //Draw debug line
            Debug.DrawRay(start.transform.position,dir*BlockDistance,Color.yellow);

            //Check collision
            RaycastHit hit;
            if(Physics.Raycast(start.transform.position,dir,out hit,BlockDistance)){ //使用射线检测距离
                if(!IsBodyPart(control,hit.collider) //如果不是身体一部分
                    &&!IsIgnoreCharacter(control,hit.collider)
                    // &&!Ledge.IsLedge(hit.collider.gameObject)
                    &&!Ledge.IsLedgeChecker(hit.collider.gameObject)
                    &&!Weapon.IsWeapon(hit.collider.gameObject)
                    &&!TrapSpikes.IsTrap(hit.collider.gameObject)){ 
                    // if(BlockingObjDic.ContainsKey(obj)){
                    //     BlockingObjDic[obj] = hit.collider.transform.root.gameObject;
                    // }else{
                    //     BlockingObjDic.Add(obj, hit.collider.transform.root.gameObject);
                    // }
                    collisionPoint=hit.point;
                    return hit.collider.transform.root.gameObject;
                }
                else {
                    return null;
                }
                // else{
                //     if(BlockingObjDic.ContainsKey(obj)){
                //         BlockingObjDic.Remove(obj);
                //     }
                // }
            }else{
                // if(BlockingObjDic.ContainsKey(obj)){
                //     BlockingObjDic.Remove(obj);
                // }
                return null;
            }
        }
        static bool IsIgnoreCharacter(CharacterControl control,Collider col){
            if(!control.animationProgress.IsIgnoreCharacterTime){
                return false;
            }else{
                CharacterControl blockingChar=CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);
                if(blockingChar==null){
                    return false;
                }
                if(blockingChar==control){
                    return false;
                }
                else{
                    return true;
                }
            }
        }

        static bool IsBodyPart(CharacterControl control,Collider col){
            // CharacterControl control=col.transform.root.GetComponent<CharacterControl>();
            // if(control==null){ //如果不是身体一部分
            //     return false;
            // }
            // if(control.gameObject==col.gameObject){ //如果collider是角色控件自己，即不是身体部分，而是root
            //     return false;
            // }
            // if(control.RagdollParts.Contains(col)){ //在布娃娃部件列表内，即是自己的部件
            //     return true;
            // }
            // return false;

            if(col.transform.root.gameObject==control.gameObject){ //如果是同一部件
                return true;
            }

            //如果不是，就可能是敌人
            CharacterControl target=CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);
            
            if(target==null){ //无CharacterControl，非身体部位
                return false;
            }

            if(target.DAMAGE_DATA.IsDead()){ //确认是否是死人
                return true;
            }else{
                return false;
            }
        }

    }
}