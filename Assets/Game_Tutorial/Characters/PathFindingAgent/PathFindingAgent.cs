using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting;

namespace Games_tutorial
{
    public class PathFindingAgent : MonoBehaviour
    {
        public GameObject target;
        public bool TargetPlayableCharacter;
        NavMeshAgent navMeshAgent;
        Coroutine MoveRoutine;

        //public Vector3 StartPosition;
        //public Vector3 EndPosition;
        //List<Coroutine> MoveRoutines=new List<Coroutine>(); //记录运动轨迹

        public GameObject StartSphere;
        public GameObject EndSphere;
        public bool StartWalk;

        public CharacterControl owner=null;

        private void Awake(){
            navMeshAgent=GetComponent<NavMeshAgent>();
        }

        public void GoToTarget(){
            navMeshAgent.enabled=true;

            StartSphere.transform.parent=null;
            EndSphere.transform.parent=null;
            StartWalk=false;

            navMeshAgent.isStopped=false;

            if(TargetPlayableCharacter){
                target=CharacterManager.Instance.GetPlayableCharacter().gameObject;
            }
            navMeshAgent.SetDestination(target.transform.position);
        
            // if(MoveRoutines.Count!=0){
            //     if(MoveRoutines[0]!=null){
            //         StopCoroutine(MoveRoutines[0]);
            //     }
            //     MoveRoutines.RemoveAt(0);
            // }

            // MoveRoutines.Add(StartCoroutine(_Move()));

            MoveRoutine=StartCoroutine(_Move());
        }

        private void OnEnable(){
            if(MoveRoutine!=null){
                StopCoroutine(MoveRoutine);
            }
        }

        IEnumerator _Move(){
            while(true){
                if(navMeshAgent.isOnOffMeshLink){
                    //owner.navMeshObstacle.carving=true;

                    //StartPosition=transform.position; //记录开始位置
                    StartSphere.transform.position=navMeshAgent.currentOffMeshLinkData.startPos; //直接使用naVMeshAgent记录的始末位置，无需跳到下一帧
                    EndSphere.transform.position=navMeshAgent.currentOffMeshLinkData.endPos;
                    
                    navMeshAgent.CompleteOffMeshLink(); //移动到下一个Link上去
                    
                    //yield return new WaitForEndOfFrame(); //进入下一帧

                    //EndPosition=transform.position; //记录结束位置
                    
                    navMeshAgent.isStopped=true; //停止移动
                    StartWalk=true;

                    //yield break; 
                    break;
                }
                
                Vector3 dist=transform.position - navMeshAgent.destination;
                if(Vector3.SqrMagnitude(dist)<0.5f){
                    // if(Vector3.SqrMagnitude(owner.transform.position-navMeshAgent.destination)>1f){
                    //     owner.navMeshObstacle.carving=true;
                    // }
                    //StartPosition=transform.position;
                    StartSphere.transform.position=navMeshAgent.destination;
                    //EndPosition=transform.position;
                    EndSphere.transform.position=navMeshAgent.destination;
                    navMeshAgent.isStopped=true;
                    StartWalk=true;
                    break;
                }
                
                yield return new WaitForEndOfFrame(); //游戏继续
            }

            yield return new WaitForSeconds(0.5f); // 不在角色停止移动时立刻开始carving，给予一些时间

            owner.navMeshObstacle.carving=true;
        }
    }
}