using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class VerticalVelocity : SubComponent
    {
        public VerticalVelocityData verticalVelocityData;

        void Start() {
            verticalVelocityData=new VerticalVelocityData {
                NoJumpCancel=false,
                MaxWallSideVelocity=Vector3.zero,
            };
            subComponentProcessor.verticalVelocityData=verticalVelocityData;
            subComponentProcessor.ComponentsDic.Add(SubComponentType.VERTICAL_VELOCITY,this);
        }
        public override void OnFixedUpdate() {
            // jump cancel after letting go
            // bool cancelPull=AIR_CONTROL.GetBool((int)AirControlBool.CANCEL_PULL);
            if(!verticalVelocityData.NoJumpCancel) {
                // if(RIGID_BODY.velocity.y<0f){ //向下
                //     RIGID_BODY.velocity+=-Vector3.up*GravityMultipilier;
                // }
                if(control.RIGID_BODY.velocity.y > 0f && !control.Jump) {
                    // RIGID_BODY.velocity+=-Vector3.up*PullMultipilier;
                    control.RIGID_BODY.velocity -= Vector3.up * control.RIGID_BODY.velocity.y * 0.1f; //可以通过跳跃键摁下时间控制跳跃高度
                }
            }
            

            // if(animationProgress.RagdollTriggered) {
            //     TurnOnRagdoll();
            //     animationProgress.RagdollTriggered = false;
            // }

            // Vector3 maxFallVelocity = AIR_CONTROL.GetVector3((int)AirControlVector3.MAX_FALL_VELOCITY);

            //slow down wallslide
            if(verticalVelocityData.MaxWallSideVelocity.y != 0f) {
                if(control.RIGID_BODY.velocity.y <= verticalVelocityData.MaxWallSideVelocity.y) {
                    control.RIGID_BODY.velocity = verticalVelocityData.MaxWallSideVelocity;
                }
            }
        }

        public override void OnUpdate() {
            
        }
    }
}