using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    public class PlayerGround:SubComponent {
        public GroundData groundData;
        void Start() {
            groundData=new GroundData {
                
            };
            subComponentProcessor.groundData=groundData;
        }
        public override void OnFixedUpdate() {
            
        }

        public override void OnUpdate() {
            
        }
    }
}