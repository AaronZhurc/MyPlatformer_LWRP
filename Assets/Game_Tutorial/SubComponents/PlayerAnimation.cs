using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    public class PlayerAnimation : SubComponent
    {
        public AnimationData animationData;
        void Start()
        {
            animationData = new AnimationData {
                CurrentRunningAbilities=new Dictionary<StateData,int>(),
                IsRunning=IsRunning,
            };
            subComponentProcessor.animationData=animationData;
            subComponentProcessor.ComponentsDic.Add(SubComponentType.PLAYER_ANIMATION,this);
        }
        public override void OnFixedUpdate() {
        }

        public override void OnUpdate() {
            // if(PressTime==0f){
            //     AttackTriggered=false;
            // }else if(PressTime>MaxPressTime){
            //     AttackTriggered=false;
            // }else{
            //     AttackTriggered=true;
            // }

            if(IsRunning(typeof(LockTransition))){
                if(control.animationProgress.LockTransition){
                    control.SkinnedMeshAnimator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.LockTransition],true);
                }else{
                    control.SkinnedMeshAnimator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.LockTransition],false);
                }
            }else{
                control.SkinnedMeshAnimator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.LockTransition],false);
            }
        }

        public bool IsRunning(System.Type type) {
            // for(int i=0;i<CurrentRunningAbilities.Count;i++){
            //     if(type==CurrentRunningAbilities[i].GetType()){
            //         if(CurrentRunningAbilities[i]==self){
            //             return false;
            //         }else{
            //             //Debug.Log(type.ToString()+" is already running");
            //             return true;
            //         }
            //     }
            // }
            // return false;

            foreach(KeyValuePair<StateData,int> data in animationData.CurrentRunningAbilities){
                if(data.Key.GetType()==type){
                    return true;
                }
            }
            return false;
        }

    }
}