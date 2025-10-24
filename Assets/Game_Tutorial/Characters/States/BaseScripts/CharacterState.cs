using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class CharacterState : StateMachineBehaviour
    {
        public CharacterControl characterControl;
        [Space(10)]
        public List<StateData> ListAbilityData=new List<StateData>();
        

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(characterControl==null){
                CharacterControl control=animator.transform.root.GetComponent<CharacterControl>();
                control.CacheCharacterControl(animator);
            }
            foreach(StateData d in ListAbilityData){
                d.OnEnter(this,animator,stateInfo);

                if(characterControl.animationProgress.CurrentRunningAbilities.ContainsKey(d)){
                    characterControl.animationProgress.CurrentRunningAbilities[d]+=1;
                }else{
                    characterControl.animationProgress.CurrentRunningAbilities.Add(d,1);
                }
            }
        }

        public void UpdateAll(CharacterState characterState,Animator animator,AnimatorStateInfo stateInfo){
            foreach(StateData d in ListAbilityData){
                d.UpdateAbility(characterState,animator,stateInfo); //简单更新操作
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) //对每帧进行更新
        {
            UpdateAll(this,animator,stateInfo);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach(StateData d in ListAbilityData){
                d.OnExit(this,animator,stateInfo);
                if(characterControl.animationProgress.CurrentRunningAbilities.ContainsKey(d)){
                    characterControl.animationProgress.CurrentRunningAbilities[d]-=1;
                }
                if(characterControl.animationProgress.CurrentRunningAbilities[d]<=0){
                    characterControl.animationProgress.CurrentRunningAbilities.Remove(d);
                }
            }   
        }

        // private CharacterControl characterControl;
        // public CharacterControl GetCharacterControl(Animator animator){
        //     if (characterControl==null){
        //         //characterControl=animator.GetComponentInParent<CharacterControl>();
        //         characterControl=animator.transform.root.GetComponent<CharacterControl>(); //角色控制会被放在最顶层，因此可以找到
        //     }
        //     return characterControl;
        // }
    }
}