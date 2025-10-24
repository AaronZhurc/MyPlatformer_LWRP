using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class AttackInfo : MonoBehaviour
    {
        public CharacterControl Attacker=null; //攻击者
        public Attack AttackAbility; 
        //public List<string> ColliderNames=new List<string>(); //携带攻击信息的身体部件  
        // public bool LaunchIntoAir;
        public List<AttackPartType> AttackParts=new List<AttackPartType>();
        public DeathType deathType;
        public bool MustCollide; //条件 必须碰撞  
        public bool MustFaceAttacker; //必须面对攻击者  
        public float LethalRange; //攻击范围
        public int MaxHits; //最大攻击数
        public int CurrentHits; //当前攻击数  
        public bool isRegistered; //当前攻击是否已被注册  
        public bool isFinished; //当前动画是否完播
        public bool UseRagdollDeath;
        public List<CharacterControl> RegisteredTargets=new List<CharacterControl>();


        public void ResetInfo(Attack attack,CharacterControl attacker){
            isRegistered=false;
            isFinished=false;
            AttackAbility=attack;
            Attacker=attacker;
            // UseRagdollDeath=attack.UseRagdollDeath;
            RegisteredTargets.Clear();
        }

        public void Register(Attack attack){
            isRegistered=true;

            AttackAbility=attack;
            //ColliderNames=attack.ColliderNames;
            AttackParts=attack.AttackParts;
            // LaunchIntoAir=attack.LaunchIntoAir;
            // deathType=attack.deathType;

            MustCollide=attack.MustCollide;
            MustFaceAttacker=attack.MustFaceAttacker;
            LethalRange=attack.LethalRange;
            MaxHits=attack.MaxHits;
            CurrentHits=0;
        }

        private void OnDisable(){
            isFinished=true;
        }
    }
}