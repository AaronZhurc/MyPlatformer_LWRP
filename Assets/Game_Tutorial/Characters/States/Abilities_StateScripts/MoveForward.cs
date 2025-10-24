using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "MoveForward", menuName = "Games/AbilityData/MoveForward")]
    public class MoveForward : StateData
    {
        public bool debug;

        public bool AllowEarlyTurn;
        public bool LockDirection;
        public bool LockDirectionNextState;
        public bool Constant;
        public AnimationCurve SpeedGraph;
        public float Speed;
        public float BlockDistance;

        [Header("IgnoreCharacterBox")]
        public bool IgnoreCharacterBox;
        public float IgnoreStartTime;
        public float IgnoreEndTime;

        [Header("Momentum")]
        public bool UseMomentum;
        public float StartingMomentum;
        public float MaxMomentum;
        public bool ClearMomentumOnExit;

        [Header("MoveOnHit")]
        public bool MoveOnHit;

        // private List<GameObject> SpheresList;
        // private float DirBlock;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            
            characterState.characterControl.animationProgress.LatestMoveForward=this;
            
            if(AllowEarlyTurn&&!control.animationProgress.LockDirectionNextState){
                if(AllowEarlyTurn&&!control.animationProgress.disallowEarlyTurn){
                    if(control.MoveLeft){
                        control.FaceForward(false);
                    }else{
                        control.FaceForward(true);
                    }
                }
            }else{
                control.animationProgress.LockDirectionNextState = false;
            }
            control.animationProgress.disallowEarlyTurn=false;
            //control.animationProgress.AirMomentum=0f;
            if(StartingMomentum>0.001f){
                if(control.IsFacingForward()){
                    control.animationProgress.AirMomentum=StartingMomentum;
                }else{
                    control.animationProgress.AirMomentum=-StartingMomentum;
                }
            }

            control.animationProgress.disallowEarlyTurn=false;
            control.animationProgress.LockDirectionNextState=false;
            // control.animationProgress.BlockingObjs.Clear();

            UpdateMoveOnHit(control);
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(debug){
                Debug.Log(stateInfo.normalizedTime);
            }
            
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;

            control.animationProgress.LockDirectionNextState=LockDirectionNextState;
            
            // if(characterState.characterControl.animationProgress.IsRunning(typeof(MoveForward))){
            //     return;
            // }
            if(characterState.characterControl.animationProgress.LatestMoveForward!=this){
                return;
            }

            if(characterState.characterControl.animationProgress.IsRunning(typeof(WallSlide))){
                return;
            }

            // if(control.animationProgress.FrameUpdated){
            //     return;
            // }
            
            // control.animationProgress.FrameUpdated=true;

            UpdateCharacterIgnoreTime(control,stateInfo);

            
            if(control.Jump){
                if(control.animationProgress.Ground!=null){
                    animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Jump],true);
                }
            }
            if(control.Turbo){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turbo],true);
            }else{
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Turbo],false);
            }
            if(UseMomentum){
                UpdateMomentum(control,stateInfo);
            }else{
                if(Constant){
                    ConstantMove(control,animator,stateInfo);
                }else{
                    ControlledMove(control,animator,stateInfo);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // CharacterControl control=characterState.GetCharacterControl(animator);
            CharacterControl control=characterState.characterControl;
            if(ClearMomentumOnExit){
                control.animationProgress.AirMomentum=0f;
            }
        }

        void UpdateMoveOnHit(CharacterControl control){
            if(!MoveOnHit){
                return;
            }
            if(control.animationProgress.Attacker!=null){
                Vector3 dir=control.transform.position-control.animationProgress.Attacker.transform.position;
                if(dir.z>0f){
                    if(Speed<0f){
                        Speed*=-1f;
                    }
                }else if(dir.z<0f){
                    if(Speed>0f){
                        Speed*=-1f;
                    }
                }
            }
        }

        private void UpdateMomentum(CharacterControl control,AnimatorStateInfo stateInfo){           
            if(!control.animationProgress.RightSideIsBlocked()){
                if(control.MoveRight){
                    control.animationProgress.AirMomentum+=SpeedGraph.Evaluate(stateInfo.normalizedTime)*Speed*Time.deltaTime;
                }
            }
            if(!control.animationProgress.LeftSideIsBlocked()){
                if(control.MoveLeft){
                    control.animationProgress.AirMomentum-=SpeedGraph.Evaluate(stateInfo.normalizedTime)*Speed*Time.deltaTime;
                }
            }

            if(control.animationProgress.RightSideIsBlocked()||control.animationProgress.LeftSideIsBlocked()){
                //如果两遍都被阻挡，动量下降到0
                control.animationProgress.AirMomentum=Mathf.Lerp(control.animationProgress.AirMomentum,0f,Time.deltaTime*1.5f);
            }

            if(Mathf.Abs(control.animationProgress.AirMomentum)>=MaxMomentum){
                if(control.animationProgress.AirMomentum>0f){
                    control.animationProgress.AirMomentum=MaxMomentum;
                }else if(control.animationProgress.AirMomentum<0f){
                    control.animationProgress.AirMomentum=-MaxMomentum;
                }
            }
            if(control.animationProgress.AirMomentum>0f){
                control.FaceForward(true);
            }else if(control.animationProgress.AirMomentum<0f){
                control.FaceForward(false);
            }
            if(!IsBlocked(control,Speed,stateInfo)){
                control.MoveForward(Speed,Mathf.Abs(control.animationProgress.AirMomentum));
            }
        }
        private void ConstantMove(CharacterControl control, Animator animator, AnimatorStateInfo stateInfo){ //自动前进，比如出拳时的自动前进
            if(!IsBlocked(control,Speed,stateInfo)){
                control.MoveForward(Speed,SpeedGraph.Evaluate(stateInfo.normalizedTime));
            }
            if(!control.MoveRight&&!control.MoveLeft){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],false);
            }else{
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],true);
            }
        }

        private void ControlledMove(CharacterControl control, Animator animator, AnimatorStateInfo stateInfo){
            if(control.MoveRight&&control.MoveLeft){ //我们希望前进代码也能给敌人使用，因此使用InputManager有点问题
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],false);
                return;
            }
            if(!control.MoveRight&&!control.MoveLeft){
                animator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.Move],false);
                return;
            }
            if (control.MoveRight){
                if(!IsBlocked(control,Speed,stateInfo)){ 
                    control.MoveForward(Speed,SpeedGraph.Evaluate(stateInfo.normalizedTime)); //为了能让前跳不匀速，我们让其乘上速度曲线
                }
            }
            if (control.MoveLeft){
                if(!IsBlocked(control,Speed,stateInfo)){
                   control.MoveForward(Speed,SpeedGraph.Evaluate(stateInfo.normalizedTime)); //注意此时角色已经旋转，所以不需要负号  
                }
            }

            CheckTurn(control);
        }

        private void CheckTurn(CharacterControl control){
            if(!LockDirection){
                if (control.MoveRight){
                    control.transform.rotation=Quaternion.Euler(0f,0f,0f); //无论前面有什么阻挡，都应当可以旋转
                }
                if (control.MoveLeft){
                    control.transform.rotation=Quaternion.Euler(0f,180f,0f);
                }
             }
        }

        void UpdateCharacterIgnoreTime(CharacterControl control, AnimatorStateInfo stateInfo){
            if(!IgnoreCharacterBox){
                control.animationProgress.IsIgnoreCharacterTime=false;
            }
            if(stateInfo.normalizedTime>IgnoreStartTime&&stateInfo.normalizedTime<IgnoreEndTime){
                control.animationProgress.IsIgnoreCharacterTime=true;
            }else{
                control.animationProgress.IsIgnoreCharacterTime=false;
            }
        }
           

        bool IsBlocked(CharacterControl control, float speed, AnimatorStateInfo stateInfo){
            // if(speed>0){
            //     SpheresList=control.collisionSpheres.FrontSpheres;
            //     DirBlock=0.3f;
            // }else{
            //     SpheresList=control.collisionSpheres.BackSpheres;
            //     DirBlock=-0.3f;
            // }

            // foreach(GameObject o in SpheresList){
            //     /*Self=false;*/
            //     Debug.DrawRay(o.transform.position,control.transform.forward*DirBlock,Color.yellow);
            //     RaycastHit hit;
            //     if(Physics.Raycast(o.transform.position,control.transform.forward*DirBlock,out hit,BlockDistance)){ //使用射线检测距离
            //         if(!IsBodyPart(hit.collider,control) //如果不是身体一部分
            //             &&!IgnoringCharacterBox(hit.collider,stateInfo)
            //             &&!Ledge.IsLedge(hit.collider.gameObject)
            //             &&!Ledge.IsLedgeChecker(hit.collider.gameObject)){ 
            //             control.animationProgress.BlockingObj=hit.collider.transform.root.gameObject;
            //             return true;
            //         }
            //     }
            //         /*上述代码和这一段做了相同的事情
            //         foreach(Collider c in control.RagdollParts){
            //             if(c.gameObject==hit.collider.gameObject){ //我们不希望射线同自身的布娃娃collider交互
            //                 Self=true;
            //                 break;
            //             }
            //         }
            //         if(!Self){
            //             return true;
            //         }*/
            // }
            // control.animationProgress.BlockingObj=null;
            // return false;

            if(control.animationProgress.FrontBlockingObjs.Count!=0){
                return true;
            }else{
                return false;
            }

        }
    }
}