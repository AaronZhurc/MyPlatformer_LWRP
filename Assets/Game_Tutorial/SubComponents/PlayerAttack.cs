using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    public class PlayerAttack:SubComponent {
        public AttackData attackData;
        void Start() {
            attackData=new AttackData {
                AttackTriggered=false,
                AttackButtonIsReset=false,
            };
            subComponentProcessor.attackData=attackData;
            subComponentProcessor.ComponentsDic.Add(SubComponentType.PLAYER_ATTACK,this);
        }
        public override void OnFixedUpdate() {
        }

        public override void OnUpdate() {
            if(control.Attack){
                // PressTime+=Time.deltaTime;
                if(attackData.AttackButtonIsReset){
                    attackData.AttackTriggered=true;
                    attackData.AttackButtonIsReset=false;
                }
            }else{
                // PressTime=0f;
                attackData.AttackButtonIsReset=true;
                attackData.AttackTriggered=false;
            }
        }
    }
}