using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "AITransition", menuName = "Games/AI/AITransitionCondition")]
    public class AITransitionCondition : StateData
    {
        public enum AITransitionType{
            RUN_TO_WALK,
            WALK_TO_RUN,
        }

        public AITransitionType aITransition;
        public AI_TYPE NextAI;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);

            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            Vector3 dist=control.aiProgress.pathfindingAgent.StartSphere.transform.position-control.transform.position;

            if(TransitionToNextAI(control)){
                control.aiController.TriggerAI(NextAI);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        bool TransitionToNextAI(CharacterControl control){
            Vector3 dist=control.aiProgress.pathfindingAgent.StartSphere.transform.position-control.transform.position;
            if(aITransition==AITransitionType.RUN_TO_WALK){
                if(Vector3.SqrMagnitude(dist)<5f){
                    return true;
                }
            }else if(aITransition==AITransitionType.WALK_TO_RUN){
                if(Vector3.SqrMagnitude(dist)>5f){
                    return true;
                }
            }
            return false;
        }
    }
}

