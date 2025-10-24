using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "FallPlatform", menuName = "Games/AI/FallPlatform")]
    public class FallPlatform : StateData
    {
        //
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            
            if(!control.SkinnedMeshAnimator.GetBool(HashManager.Instance.DicMainParams[TransitionParameter.Grounded])){
                return;
            }

            if(control.Attack){
                return;
            }

            // if(control.IsFacingForward()){
            //     if(control.transform.position.z<control.aiProgress.pathfindingAgent.EndSphere.transform.position.z){
            //         control.MoveRight=true;
            //         control.MoveLeft=false;
            //     }
            //     // else{
            //     //     control.MoveRight=false;
            //     //     control.MoveLeft=false;

            //     //     // animator.gameObject.SetActive(false);
            //     //     // animator.gameObject.SetActive(true);
            //     //     control.aiController.InitializeAI();
            //     // }
            // }else{
            //     if(control.transform.position.z>control.aiProgress.pathfindingAgent.EndSphere.transform.position.z){
            //         control.MoveRight=false;
            //         control.MoveLeft=true;
            //     }
            //     // else{
            //     //     control.MoveRight=false;
            //     //     control.MoveLeft=false;

            //     //     // animator.gameObject.SetActive(false);
            //     //     // animator.gameObject.SetActive(true);
            //     //     control.aiController.InitializeAI();
            //     // }
            // }
            if (control.transform.position.z<control.aiProgress.pathfindingAgent.EndSphere.transform.position.z){
                control.MoveRight=true;
                control.MoveLeft=false;
            }else if(control.transform.position.z>control.aiProgress.pathfindingAgent.EndSphere.transform.position.z){
                control.MoveRight=false;
                control.MoveLeft=true;
            }
            if (control.aiProgress.AIDistanceToStartSphere() > 3f)
            {
                control.Turbo = true;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

    }
}

