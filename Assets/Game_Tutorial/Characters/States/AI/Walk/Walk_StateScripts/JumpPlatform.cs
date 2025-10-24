using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "JumpPlatform", menuName = "Games/AI/JumpPlatform")]
    public class JumpPlatform : StateData
    {
        //
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            
            control.Jump=true;
            control.MoveUp=true;
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            
            if(control.Attack){
                return;
            }
            
            // float topDist=control.aiProgress.pathfindingAgent.EndSphere.transform.position.y
            //               -control.collisionSpheres.FrontSpheres[1].transform.position.y;

            float platformDist=control.aiProgress.pathfindingAgent.EndSphere.transform.position.y
                          -control.collisionSpheres.FrontSpheres[0].transform.position.y;

            if(platformDist>0.5f){
                if(control.aiProgress.pathfindingAgent.StartSphere.transform.position.z
                   <control.aiProgress.pathfindingAgent.EndSphere.transform.position.z){
                    control.MoveRight=true;
                    control.MoveLeft=false;
                }else{
                    control.MoveRight=false;
                    control.MoveLeft=true;
                }
            }   

            if(platformDist<0.1f){
                control.MoveRight=false;
                control.MoveLeft=false;
                control.Jump=false;
                control.MoveUp=false;

                // animator.gameObject.SetActive(false);
                // animator.gameObject.SetActive(true);
                // control.aiController.InitializeAI();
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

    }
}