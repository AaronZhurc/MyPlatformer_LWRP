using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class AttackManager : Singleton<AttackManager>
    {
        public List<AttackCondition> CurrentAttacks=new List<AttackCondition>(); //存储所有当前攻击的完整列表
    
        public void ForceDeregister(CharacterControl control){
            foreach(AttackCondition info in CurrentAttacks){
                if(info.Attacker == control){
                    info.isFinished=true;
                info.GetComponent<PoolObject>().TurnOff();
                }
            }
        }
    
    }
}