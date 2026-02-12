using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Games_tutorial
{
    public class DamageDetector : SubComponent //比较碰撞信息与正在注册的攻击，改脚本会放在角色控制层次结构最顶层的路径中
    {

        public DamageData damageData;

        // GeneralBodyPart DamegedPart;
        //public int DamegeTaken;
        [SerializeField]
        private float hp;

        [SerializeField]
        List<RuntimeAnimatorController> HitReactionList=new List<RuntimeAnimatorController>();
        

        [Header("Insta Kill")]
        public RuntimeAnimatorController Assassination_A;
        public RuntimeAnimatorController Assassination_B;

        [Header("Attack")]
        public Attack MarioStampAttack;
        public Attack CrowbarThrow;
        void Start() {
            damageData=new DamageData {
                Attacker=null,
                DamagedTrigger=null,
                AttackingPart=null,
                Attack=null,
                BlockedAttack=null,
                IsDead=IsDead,
            };
            subComponentProcessor.damageData=damageData;
            subComponentProcessor.ComponentsDic.Add(SubComponentType.DAMAGE_DETECTOR,this);
        }

        private bool AttackIsValid(AttackInfo info){
            if(info==null){
                return false;
            }
            if(!info.isRegistered){
                return false;
            }           
            if(info.isFinished){
                return false;
            }
            if(info.CurrentHits>=info.MaxHits){
                return false;
            }
            if(info.Attacker==control){
                return false;
            }
            if(info.MustFaceAttacker){
                Vector3 vec=this.transform.position-info.Attacker.transform.position;
                if(vec.z*info.Attacker.transform.forward.z<0f){//没有面对
                    return false;
                }
            }
            
            if(info.RegisteredTargets.Contains(this.control)){
                return false;
            }

            return true;
        }
        private void CheckAttack(){
            foreach (AttackInfo info in AttackManager.Instance.CurrentAttacks)
            {
                if(AttackIsValid(info)){
                    if(info.MustCollide){
                        if(control.animationProgress.CollidingBodyParts.Count!=0){
                            if(IsColllided(info)){
                                TakeDamage(info);
                            }
                        }
                    }else{ //AOE
                        if(IsInLethalRange(info)){
                            TakeDamage(info);
                        }
                    }
                }
            }
        }
        private bool IsColllided(AttackInfo info){
            foreach(KeyValuePair<TriggerDetector,List<Collider>> data in control.animationProgress.CollidingBodyParts){
                foreach(Collider collider in data.Value){
                    foreach(AttackPartType part in info.AttackParts){

                        if(info.Attacker.GetAttackingPart(part)==collider.gameObject){
                            damageData.SetData(info.Attacker,info.AttackAbility,data.Key,info.Attacker.GetAttackingPart(part));
                            // damageData.Attack=info.AttackAbility;
                            // damageData.Attacker=info.Attacker;
                            // damageData.DamagedTrigger=data.Key;
                            // damageData.AttackingPart=info.Attacker.GetAttackingPart(part);
                            return true;
                        }
                        
                    }
                }
            }
            // foreach(TriggerDetector trigger in control.GetAllTrigers()){
            //     foreach(Collider collider in trigger.CollidingParts){
            //         foreach(AttackPartType part in info.AttackParts){

            //             if(info.Attacker.GetAttackingPart(part)==collider.gameObject){
            //                 control.animationProgress.Attack=info.AttackAbility;
            //                 control.animationProgress.Attacker=info.Attacker;
            //                 control.animationProgress.DamagedTrigger=trigger;
            //                 control.animationProgress.AttackingPart=info.Attacker.GetAttackingPart(part);
            //                 return true;
            //             }
                        
            //         }
            //     }
            // }
            return false;
        }

        private bool IsInLethalRange(AttackInfo info){
            foreach(Collider c in control.RAGDOLL_DATA.BodyParts){
                float dist=Vector3.SqrMagnitude(c.transform.position-info.Attacker.transform.position);
                //Debug.Log(this.gameObject.name+" dist: "+dist.ToString());
                if(dist<=info.LethalRange){
                    // damageData.Attack=info.AttackAbility;
                    // damageData.Attacker=info.Attacker;

                    int index=UnityEngine.Random.Range(0, control.RAGDOLL_DATA.BodyParts.Count);
                    TriggerDetector triggerDetector=control.RAGDOLL_DATA.BodyParts[index].GetComponent<TriggerDetector>();
                    // damageData.DamagedTrigger=control.RAGDOLL_DATA.BodyParts[index].GetComponent<TriggerDetector>();
                    damageData.SetData(info.Attacker,info.AttackAbility,triggerDetector,null);
                    return true;
                }
            }
            return false;
        }

        bool IsDead(){
            if(hp<=0f){
                return true;
            }else{
                return false;
            }
        }

        bool IsBlocked(AttackInfo info) {
            if(info == damageData.BlockedAttack && damageData.BlockedAttack != null) {
                return true;
            }
            if(control.animationProgress.IsRunning(typeof(Block))) {
                Vector3 dir = info.Attacker.transform.position - control.transform.position;
                if(dir.z > 0f) {
                    if(control.IsFacingForward()) {
                        return true;
                    }
                } else if(dir.z < 0f) {
                    if(!control.IsFacingForward()) {
                        return true;
                    }
                }
            }
            return false;
        }
        public void TakeDamage(AttackInfo info){
            if(IsDead()){
                if(!info.RegisteredTargets.Contains(this.control)){
                    info.RegisteredTargets.Add(this.control);
                    control.AddForceToDamagePart(true);
                }
                
                return;
            }
            if(IsBlocked(info)) {
                damageData.BlockedAttack = info;
                return;
            }
            if(info.MustCollide) {
                    CameraManager.Instance.ShakeCamera(0.35f);

                    if(info.AttackAbility.UseDeathParticles) {
                        if(info.AttackAbility.ParticleType.ToString().Contains("VFX")) {
                            GameObject vfx = PoolManager.Instance.GetObject(info.AttackAbility.ParticleType);

                            vfx.transform.position = damageData.AttackingPart.transform.position;
                            vfx.SetActive(true);
                            if(info.Attacker.IsFacingForward()) {
                                vfx.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                            }
                            else {
                                vfx.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            }
                        }
                    }
                } 
            //Debug.Log(info.Attacker.gameObject.name+" hits: "+this.gameObject.name);
            //Debug.Log(this.gameObject.name+" hit "+DamegedPart.ToString());
            
            info.CurrentHits++;
            // DamegeTaken++;
            hp-=info.AttackAbility.Damage;
            
            // if(!info.UseRagdollDeath){
            //     //control.SkinnedMeshAnimator.runtimeAnimatorController=info.AttackAbility.GetDeathAnimator();
            //     control.SkinnedMeshAnimator.runtimeAnimatorController=DeathAnimationManager.Instance.GetAnimator(DamegedPart, info);
            //     // control.CacheCharacterControl(control.SkinnedMeshAnimator);
            // }else{
                   // control.animationProgress.RagdollTriggered=true;
            // }

            AttackManager.Instance.ForceDeregister(control);

            control.animationProgress.CurrentRunningAbilities.Clear();

            if(IsDead()){
                control.RAGDOLL_DATA.RagdollTriggered=true;

                //control.animationProgress.RagdollTriggered=true;
                //control.TurnOnRagdoll();

                //control.GetComponent<BoxCollider>().enabled=false;
                //control.ledgeChecker.GetComponent<BoxCollider>().enabled=false;
                
                // control.ProcDic[CharacterProc.LEDGE_COLLIDERS_OFF]();
                // control.RIGID_BODY.useGravity=false;

                // if(control.aiController!=null){
                //     control.aiController.gameObject.SetActive(false);
                //     control.navMeshObstacle.enabled=false;
                // }
            }else{
                int rand=UnityEngine.Random.Range(0,HitReactionList.Count);
                control.SkinnedMeshAnimator.runtimeAnimatorController=null; //多次击中时有用
                control.SkinnedMeshAnimator.runtimeAnimatorController=HitReactionList[rand];
            }
            
            if(!info.RegisteredTargets.Contains(this.control)){
                info.RegisteredTargets.Add(this.control);
            }
        }

        public void TriggerSpikeDeath(RuntimeAnimatorController animator){
            control.SkinnedMeshAnimator.runtimeAnimatorController = animator;
        }

        public void DeathBySpikes(){
            damageData.DamagedTrigger = null; //不对身体部位添加任何力
            hp =0f;
        }

        public void DeathByInstaKill(CharacterControl attacker){
            control.animationProgress.CurrentRunningAbilities.Clear();
            attacker.animationProgress.CurrentRunningAbilities.Clear();

            control.RIGID_BODY.useGravity=false;
            control.boxCollider.enabled=false;
            control.SkinnedMeshAnimator.runtimeAnimatorController=Assassination_B;

            attacker.RIGID_BODY.useGravity=false;
            attacker.boxCollider.enabled=false;
            attacker.SkinnedMeshAnimator.runtimeAnimatorController=Assassination_A;
            
            Vector3 dir=control.transform.position-attacker.transform.position;

            if(dir.z < 0f) {
                attacker.FaceForward(false);
            }else if(dir.z>0f){
                attacker.FaceForward(true);
            }
            
            control.transform.LookAt(control.transform.position+(attacker.transform.forward*5f),Vector3.up);
            control.transform.position=attacker.transform.position+attacker.transform.forward*0.45f;
            
            hp =0f;
        }

        public override void OnUpdate() {
            if(AttackManager.Instance.CurrentAttacks.Count>0){
                CheckAttack();
            }
        }

        public override void OnFixedUpdate() {
        }
    }
}