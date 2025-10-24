using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class DeathAnimationManager : Singleton<DeathAnimationManager>
    {
        DeathAnimationLoader deathAnimationLoader;
        List<RuntimeAnimatorController> Candidates=new List<RuntimeAnimatorController>();
        void SetupDeathAnimatorLoader(){
            if(deathAnimationLoader==null){
                GameObject obj=Instantiate(Resources.Load("DeathAnimationLoader",typeof(GameObject)) as GameObject);
                DeathAnimationLoader loader=obj.GetComponent<DeathAnimationLoader>();

                deathAnimationLoader=loader;
            }
        }

        // public RuntimeAnimatorController GetAnimator(GeneralBodyPart generalBodyPart, AttackInfo info){
        //     SetupDeathAnimatorLoader();
        //     Candidates.Clear();
        //     foreach (DeathAnimationData data in deathAnimationLoader.DeathAnimationDataList) //此处即根据攻击类型找出我们应该触发什么动画
        //     {
        //         if(info.deathType==data.deathType){
        //             if(info.deathType!=DeathType.None){
        //                 Candidates.Add(data.Animator);
        //             }else if(!info.MustCollide){ 
        //                 Candidates.Add(data.Animator);
        //             }else{
        //                 foreach (GeneralBodyPart part in data.GeneralBodyParts)
        //                 {
        //                     if(part==generalBodyPart){
        //                         Candidates.Add(data.Animator);
        //                         break;
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //     return Candidates[Random.Range(0,Candidates.Count)];
        // }
    }
}