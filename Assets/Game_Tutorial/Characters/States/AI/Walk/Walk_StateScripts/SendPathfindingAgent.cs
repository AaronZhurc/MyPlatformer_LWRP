using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Games_tutorial
{
    public enum AI_Walk_Transitions{
        start_walking,
        jump_platform,
        fall_platform,
    }

    [CreateAssetMenu(fileName = "SendPathfindingAgent", menuName = "Games/AI/SendPathfindingAgent")]
    public class SendPathfindingAgent : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            if(control.aiProgress.pathfindingAgent == null){
                GameObject p = Instantiate(Resources.Load("PathfindingAgent",typeof(GameObject)) as GameObject);
                control.aiProgress.pathfindingAgent=p.GetComponent<PathFindingAgent>();
            }

            control.aiProgress.pathfindingAgent.owner=control;
            control.aiProgress.pathfindingAgent.GetComponent<NavMeshAgent>().enabled=false;
            control.aiProgress.pathfindingAgent.transform.position=control.transform.position+Vector3.up*0.5f; //增加一些矢量，考虑就算AI在较高层，agent也是从底部开始 
            control.navMeshObstacle.carving=false;
            control.aiProgress.pathfindingAgent.GoToTarget();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control = characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            if(control.aiProgress.pathfindingAgent.StartWalk){
                animator.SetBool(HashManager.Instance.DicAITrans[AI_Walk_Transitions.start_walking],true);
                // animator.SetBool(AI_Walk_Transitions.start_running.ToString(),true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.DicAITrans[AI_Walk_Transitions.start_walking],false);
            // animator.SetBool(AI_Walk_Transitions.start_running.ToString(),false);
        }

    }
}