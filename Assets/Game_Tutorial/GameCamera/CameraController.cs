using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public enum CameraTrigger{
        Default,
        Shake,
    }
    public class CameraController : MonoBehaviour
    {
        private Animator animator;
        public Animator ANIMATOR{
            get{
                if(animator==null){
                    animator=GetComponent<Animator>();
                }
                return animator;
            }
        }
        public void TriggerCamera(CameraTrigger trigger){
            ANIMATOR.SetTrigger(HashManager.Instance.DicCameraTriggers[trigger]);
        }
    }
}