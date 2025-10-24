using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "StartRunning", menuName = "Games/AI/StartRunning")]
    public class StartRunning : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            Vector3 dir=control.aiProgress.pathfindingAgent.StartSphere.transform.position-control.transform.position;

            if(dir.z>0f){
                control.FaceForward(true);
                control.MoveRight=true;
                control.MoveLeft=false;
            }else{
                control.FaceForward(false);
                control.MoveRight=false;
                control.MoveLeft=true;
            }

            // Vector3 dist=control.aiProgress.pathfindingAgent.StartSphere.transform.position-control.transform.position;
            if(control.aiProgress.AIDistanceToStartSphere()>2f){
                control.Turbo=true;
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            // Vector3 dist=control.aiProgress.pathfindingAgent.StartSphere.transform.position-control.transform.position;
            if(control.aiProgress.AIDistanceToStartSphere()<5f){
                control.MoveRight=false;
                control.MoveLeft=false;   
                control.Turbo=false;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }
}