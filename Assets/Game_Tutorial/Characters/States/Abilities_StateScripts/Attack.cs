using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public enum AttackPartType{
        LEFT_HAND,
        RIGHT_HAND,
        LEFT_FOOT,
        RIGHT_FOOT,
        MELEE_WEAPON,
    }

    [CreateAssetMenu(fileName = "Attack", menuName = "Games/AbilityData/Attack")]
    public class Attack : StateData
    {
        public bool debug;
        public float StartAttackTime;
        public float EndAttackTime;
        //public List<string> ColliderNames=new List<string>(); //每个攻击记录使用到的身体部位
        //public bool LaunchIntoAir;
        public List<AttackPartType> AttackParts=new List<AttackPartType>();
        // public DeathType deathType;
        public bool MustCollide; //条件 必须碰撞  
        public bool MustFaceAttacker; //必须面对攻击者  
        public float LethalRange; //攻击范围
        public int MaxHits; //最大攻击数
        public float Damage;

        [Header("Combo")]
        public float ComboStartTime;
        public float ComboEndTime;

        [Header("Ragdoll Death")]
        // public bool UseRagdollDeath;
        public float ForwardForce;
        public float RightForce;
        public float UpForce;

        [Header("Death Particles")]
        public bool UseDeathParticles;
        public PoolObjectType ParticleType;


        //public List<RuntimeAnimatorController> DeathAnimators=new List<RuntimeAnimatorController>(); //一组相关死亡动画
        private List<AttackInfo> FinishedAttacks=new List<AttackInfo>();

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.animationProgress.AttackTriggered=false;
            
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Attack],false);

            // GameObject obj=Instantiate(Resources.Load("AttackInfo",typeof(GameObject))) as GameObject; //将预制件AttackInfo实例化 //我们拥有pool系统之后就不需要实例化了
            GameObject obj=PoolManager.Instance.GetObject(PoolObjectType.ATTACKINFO);
            AttackInfo info=obj.GetComponent<AttackInfo>();

            obj.SetActive(true); //获取后打开

            // info.ResetInfo(this,characterState.GetCharacterControl(animator)); //初始化
            info.ResetInfo(this,characterState.characterControl);
            //在一开始使用动画对象，如果处于更新动画期间时使用动画对象，很多角色可能共享相同可编写脚本对象，攻击者可能会被混淆

            if(!AttackManager.Instance.CurrentAttacks.Contains(info)){ //如果该信息没有被攻击管理器记录，我们进行记录
                AttackManager.Instance.CurrentAttacks.Add(info);
            }
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            RegisterAttack(characterState,animator,stateInfo);
            DeregisterAttack(characterState,animator,stateInfo);
            CheckCombo(characterState,animator,stateInfo);
        }

        public void RegisterAttack(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo){
            if(StartAttackTime<=stateInfo.normalizedTime && EndAttackTime>stateInfo.normalizedTime){
                foreach (AttackInfo info in AttackManager.Instance.CurrentAttacks)
                {
                    if(info==null){
                        continue;
                    }
                    if(!info.isRegistered&&info.AttackAbility==this){ //我们希望查看尚未被注册的信息，以及只与此攻击相关的信息
                        // if(debug){
                        //     Debug.Log(this.name+"registered:"+stateInfo.normalizedTime);
                        // }
                        info.Register(this); 
                    }
                }
            }
        }

        public void DeregisterAttack(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo){
            if(stateInfo.normalizedTime>=EndAttackTime){ //动画播完
                 foreach (AttackInfo info in AttackManager.Instance.CurrentAttacks)
                {
                    if(info==null){
                        continue;
                    }
                    if(info.AttackAbility==this&&!info.isFinished){ //攻击尚未结束
                        info.isFinished=true;
                        //Destroy(info.gameObject);
                        info.GetComponent<PoolObject>().TurnOff();
                        /*if(debug){
                            Debug.Log(this.name+"registered:"+stateInfo.normalizedTime);
                        }*/
                    }
                }
            }
        }

        public void CheckCombo(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo){
            if(stateInfo.normalizedTime>= ComboStartTime){ //可以使用公共变量设置combo的开始和限制时间
                if(stateInfo.normalizedTime<ComboEndTime){
                    // CharacterControl control=characterState.GetCharacterControl(animator);
                    if(characterState.characterControl.animationProgress.AttackTriggered/*control.Attack*/){
                        //Debug.Log("upper");
                        animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Attack],true);
                        // characterState.characterControl.animationProgress.AttackTriggered=false;
                    }
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Attack],false);
            ClearAttack();
        }

        public void ClearAttack(){
            FinishedAttacks.Clear();
            foreach(AttackInfo info in AttackManager.Instance.CurrentAttacks){
                if(info==null|| info.AttackAbility==this /*info.isFinished*/){ //改为检测该攻击是否属于该能力
                    FinishedAttacks.Add(info);
                }
            }
            foreach(AttackInfo info in FinishedAttacks){
                if(AttackManager.Instance.CurrentAttacks.Contains(info)){
                    AttackManager.Instance.CurrentAttacks.Remove(info);
                }
            }
            /*
            for(int i=0;i<AttackManager.Instance.CurrentAttacks.Count;i++){
                if(AttackManager.Instance.CurrentAttacks[i]==null||AttackManager.Instance.CurrentAttacks[i].isFinished){
                    AttackManager.Instance.CurrentAttacks.RemoveAt(i); //我们不应该在此时一边遍历列表一边删除列表元素
                }
            }
            */
        }

        /*public RuntimeAnimatorController GetDeathAnimator(){
            int index=UnityEngine.Random.Range(0,DeathAnimators.Count); //随机死亡动画
            return DeathAnimators[index];
        }*/
    }
}
