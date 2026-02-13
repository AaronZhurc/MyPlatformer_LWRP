using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class PlayerRotation:SubComponent {
        public RotationData rotationData;
        static string TutorialScene_CharacterSelect="TutorialScene_CharacterSelect";
        void Start() {
            rotationData=new RotationData {
                LockEarlyTurn=false,
                LockDirectionNextState=false,
                EarlyTurnIsLocked=EarlyTurnIsLocked,
                FaceForward=FaceForward,
                IsFacingForward=IsFacingForward,
            };
            subComponentProcessor.rotationData=rotationData;
        }
        public override void OnFixedUpdate() {
            
        }

        public override void OnUpdate() {
            
        }

        bool EarlyTurnIsLocked() {
            if(rotationData.LockEarlyTurn || rotationData.LockDirectionNextState) {
                return true;
            }
            return false;
        }

        public void FaceForward(bool forward) {
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(TutorialScene_CharacterSelect)) {
                return;
            }

            if(!control.SkinnedMeshAnimator.enabled) {
                return;
            }

            if(forward) {
                control.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else {
                control.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        public bool IsFacingForward() {
            return control.transform.forward.z > 0f;
        }
    }
}