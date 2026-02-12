
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
namespace Games_tutorial
{
    [System.Serializable]
    public class DamageData
    {
        public Attack Attack;
        public CharacterControl Attacker;
        public TriggerDetector DamagedTrigger;
        public GameObject AttackingPart;
        public AttackInfo BlockedAttack;

        public delegate bool ReturnBool();
        public ReturnBool IsDead;

        public void SetData(CharacterControl attacker,Attack attack,TriggerDetector damagedTrigger,GameObject attackingPart)
        {
            Attacker=attacker;
            Attack=attack;
            DamagedTrigger=damagedTrigger;
            AttackingPart=attackingPart;
        }
        
    }
}