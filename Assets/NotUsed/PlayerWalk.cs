using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    /*
    public class PlayerWalk : CharacterState
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(VirtualInputManager.Instance.MoveRight&&VirtualInputManager.Instance.MoveLeft){
                animator.SetBool(TransitionParameter.Move.ToString(),false);
                return;
            }
             if(!VirtualInputManager.Instance.MoveRight&&!VirtualInputManager.Instance.MoveLeft){
                animator.SetBool(TransitionParameter.Move.ToString(),false);
                return;
            }
            if (VirtualInputManager.Instance.MoveRight){
                GetCharacterControl(animator).transform.Translate(Vector3.forward*GetCharacterControl(animator).Speed*Time.deltaTime);
                GetCharacterControl(animator).transform.rotation=Quaternion.Euler(0f,0f,0f);
            }
            if (VirtualInputManager.Instance.MoveLeft){
                GetCharacterControl(animator).transform.Translate(Vector3.forward*GetCharacterControl(animator).Speed*Time.deltaTime); //注意此时角色已经旋转，所以不需要负号
                GetCharacterControl(animator).transform.rotation=Quaternion.Euler(0f,180f,0f);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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