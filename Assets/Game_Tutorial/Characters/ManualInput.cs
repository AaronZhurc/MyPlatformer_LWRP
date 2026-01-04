using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class ManualInput : SubComponent
    {

        public List<InputKeyType> DoubleTaps=new List<InputKeyType>();
        private List<InputKeyType> UpKeys=new List<InputKeyType>();
        private Dictionary<InputKeyType, float> DicDoubleTapTimings=new Dictionary<InputKeyType, float>();

        void Start() {
            control.SubComponentsDic.Add(SubComponents.MANUALINPUT,this);

            control.BoolDic.Add(BoolData.DOUBLETAP_UP, IsDoubleTap_Up);
            control.BoolDic.Add(BoolData.DOUBLETAP_DOWN, IsDoubleTap_Down);
        }

        public override void OnFixedUpdate() {
            throw new System.NotImplementedException();
        }
        // Update is called once per frame
        public override void OnUpdate()
        {
            if (VirtualInputManager.Instance.Turbo){
                control.Turbo=true;
                ProcDoubleTap(InputKeyType.KEY_TURBO);
            }else{
                control.Turbo=false;
                RemoveDoubleTap(InputKeyType.KEY_TURBO);
            }
            if (VirtualInputManager.Instance.MoveUp){
                control.MoveUp=true;
                ProcDoubleTap(InputKeyType.KEY_MOVE_UP);
            }else{
                control.MoveUp=false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_UP);
            }
            if (VirtualInputManager.Instance.MoveDown){
                control.MoveDown=true;
                ProcDoubleTap(InputKeyType.KEY_MOVE_DOWN);
            }else{
                control.MoveDown=false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_DOWN);
            }
            if (VirtualInputManager.Instance.MoveRight){
                control.MoveRight=true;
                ProcDoubleTap(InputKeyType.KEY_MOVE_RIGHT);
            }else{
                control.MoveRight=false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_RIGHT);
            }
            if (VirtualInputManager.Instance.MoveLeft){
                control.MoveLeft=true;
                ProcDoubleTap(InputKeyType.KEY_MOVE_LEFT);
            }else{
                control.MoveLeft=false;
                RemoveDoubleTap(InputKeyType.KEY_MOVE_LEFT);
            }
            if (VirtualInputManager.Instance.Jump){
                control.Jump=true;
                ProcDoubleTap(InputKeyType.KEY_JUMP);
            }else{
                control.Jump=false;
                RemoveDoubleTap(InputKeyType.KEY_JUMP);
            }
            if(VirtualInputManager.Instance.Attack){
                control.Attack=true;
                ProcDoubleTap(InputKeyType.KEY_ATTACK);
            }else{
                control.Attack=false;
                RemoveDoubleTap(InputKeyType.KEY_ATTACK);
            }
            
            if(VirtualInputManager.Instance.Block){
                control.Block=true;
                ProcDoubleTap(InputKeyType.KEY_BLOCK);
            }else{
                control.Block=false;
                RemoveDoubleTap(InputKeyType.KEY_BLOCK);
            }

            //double tap running
            if(DoubleTaps.Contains(InputKeyType.KEY_MOVE_RIGHT) || DoubleTaps.Contains(InputKeyType.KEY_MOVE_LEFT)) {
                control.Turbo = true;
            }

            //double tap running turn 
            if(control.MoveLeft&&control.MoveRight){
                if(DoubleTaps.Contains(InputKeyType.KEY_MOVE_RIGHT)||DoubleTaps.Contains(InputKeyType.KEY_MOVE_LEFT)){
                    if(!DoubleTaps.Contains(InputKeyType.KEY_MOVE_RIGHT)){
                        DoubleTaps.Add(InputKeyType.KEY_MOVE_RIGHT);
                    }
                    if(!DoubleTaps.Contains(InputKeyType.KEY_MOVE_LEFT)){
                        DoubleTaps.Add(InputKeyType.KEY_MOVE_LEFT);
                    }
                }
            }
        }

        void ProcDoubleTap(InputKeyType keyType){
            if(!DicDoubleTapTimings.ContainsKey(keyType)){
                DicDoubleTapTimings.Add(keyType,0f);
            }
            if(DicDoubleTapTimings[keyType]==0f||UpKeys.Contains(keyType)){
                if(Time.time<DicDoubleTapTimings[keyType]){
                    if(!DoubleTaps.Contains(keyType)){
                        DoubleTaps.Add(keyType);
                    }
                }
                if(UpKeys.Contains(keyType)){
                    UpKeys.Remove(keyType);
                }
                DicDoubleTapTimings[keyType]=Time.time+0.18f;
            }
        }

        public bool IsDoubleTap_Up() {
            if(DoubleTaps.Contains(InputKeyType.KEY_MOVE_UP)) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool IsDoubleTap_Down() {
            if(DoubleTaps.Contains(InputKeyType.KEY_MOVE_DOWN)) {
                return true;
            }
            else {
                return false;
            }
        }


        void RemoveDoubleTap(InputKeyType keyType){
            if(DoubleTaps.Contains(keyType)){
                DoubleTaps.Remove(keyType);
            }

            if(!UpKeys.Contains(keyType)){
                UpKeys.Add(keyType);
            }
        }
    }
}