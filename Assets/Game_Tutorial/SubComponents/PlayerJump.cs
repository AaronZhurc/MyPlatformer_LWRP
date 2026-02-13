using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class PlayerJump:SubComponent {
        public JumpData jumpData;
        void Start() {
            jumpData=new JumpData {
                Jumped=false,
                CanWallJump=false,
                CheckWallBlock=false,
            };
            subComponentProcessor.jumpData=jumpData;
        }
        public override void OnFixedUpdate() {
            
        }

        public override void OnUpdate() {
            
        }
    }
}