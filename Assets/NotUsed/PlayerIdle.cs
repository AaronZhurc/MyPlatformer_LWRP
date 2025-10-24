using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    /*
    public class PlayerIdle : CharacterState
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) //玩家第一次进入动画时调用
        {
            
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) //每帧调用，现在这些动画由动作自己控制
        {
            if (VirtualInputManager.Instance.MoveRight){
                animator.SetBool(TransitionParameter.Move.ToString(),true);
            }
            if (VirtualInputManager.Instance.MoveLeft){
                animator.SetBool(TransitionParameter.Move.ToString(),true);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) //退出
        {
            
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
    */
}
