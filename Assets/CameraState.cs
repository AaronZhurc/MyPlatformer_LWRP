using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class CameraState : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) //相机状态发生变化时希望能够重置所有触发器
        {
            CameraTrigger[] arr=System.Enum.GetValues(typeof(CameraTrigger)) as CameraTrigger[];
            foreach (CameraTrigger t in arr)
            {
                CameraManager.Instance.CAM_CONTROLLER.ANIMATOR.ResetTrigger(t.ToString());
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(stateInfo.normalizedTime>0.7f){
                if(stateInfo.IsName("Shake")){
                    animator.SetTrigger(HashManager.Instance.DicCameraTriggers[CameraTrigger.Default]);
                }
            }
        }
    }
}