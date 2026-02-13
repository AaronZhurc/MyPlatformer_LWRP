using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{

    public class MomentumCalculator:SubComponent {
        public MomentumData momentumData;
        void Start() {
            momentumData=new MomentumData {
                Momentum=0f,
                CalculateMomentum=CalculateMomentum,
            };
            subComponentProcessor.momentumData=momentumData;
        }
        public override void OnFixedUpdate() {
            
        }

        public override void OnUpdate() {
            
        }

        void CalculateMomentum(float speed, float maxMomentum) {
            // current momentum
            // float momentum=control.AIR_CONTROL.GetFloat((int)AirControlFloat.AIR_MOMENTUM);

            if(!control.BLOCKING_DATA.RightSideBlocked()){
                if(control.MoveRight){
                    momentumData.Momentum+=speed;
                    // control.AIR_CONTROL.SetFloat((int)AirControlFloat.AIR_MOMENTUM,momentum+speed);
                }
            }
            if(!control.BLOCKING_DATA.LeftSideBlocked()){
                if(control.MoveLeft){
                    momentumData.Momentum-=speed; 
                    // control.AIR_CONTROL.SetFloat((int)AirControlFloat.AIR_MOMENTUM,momentum-speed);
                }
            }

            if(control.BLOCKING_DATA.RightSideBlocked()||control.BLOCKING_DATA.LeftSideBlocked()){
                //如果两遍都被阻挡，动量下降到0
                float lerped=Mathf.Lerp(momentumData.Momentum,0f,Time.deltaTime*1.5f);
                momentumData.Momentum=lerped;
                // control.AIR_CONTROL.SetFloat((int)AirControlFloat.AIR_MOMENTUM,lerped);
            }

            if(Mathf.Abs(momentumData.Momentum)>=maxMomentum){
                if(momentumData.Momentum>0f){
                    momentumData.Momentum=maxMomentum;
                }else if(momentumData.Momentum<0f){
                    momentumData.Momentum=-maxMomentum;
                }
            }
            
        }
    }
}