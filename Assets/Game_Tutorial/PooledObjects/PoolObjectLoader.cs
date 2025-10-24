using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public enum PoolObjectType{
        ATTACKINFO,
        CROWBAR_OBJ,
        CROWBAR_VFX,
        DAMAGE_WHITE_VFX,
    }
    public class PoolObjectLoader : MonoBehaviour 
    //我们不希望在每次需要攻击时实例化和销毁攻击，最好从一个对象池中获取，需要时使用，用完后放回，后面还可以再用
    {
        public static PoolObject InstantiatePrefab(PoolObjectType objType){
            GameObject obj=null;
            switch(objType){
                case PoolObjectType.ATTACKINFO:
                {
                    obj=Instantiate(Resources.Load("AttackInfo",typeof(GameObject)) as GameObject);
                    break;
                }
                case PoolObjectType.CROWBAR_OBJ:
                {
                    obj=Instantiate(Resources.Load("Crowbar",typeof(GameObject)) as GameObject);
                    break;
                }
                case PoolObjectType.CROWBAR_VFX:
                {
                    obj=Instantiate(Resources.Load("VFX_HammerDown",typeof(GameObject)) as GameObject);
                    break;
                }
                case PoolObjectType.DAMAGE_WHITE_VFX:
                {
                    obj=Instantiate(Resources.Load("VFX_Damage_White",typeof(GameObject)) as GameObject);
                    break;
                }
            }
            return obj.GetComponent<PoolObject>();
        }
    }
}