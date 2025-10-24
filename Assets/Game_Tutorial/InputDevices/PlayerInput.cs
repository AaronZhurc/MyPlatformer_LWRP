using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class PlayerInput : MonoBehaviour
    {
        public SavedKeys savedKeys;

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_TURBO])){
                VirtualInputManager.Instance.Turbo=true;
            }else{
                VirtualInputManager.Instance.Turbo=false;
            }
            if(Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_MOVE_UP])){
                VirtualInputManager.Instance.MoveUp=true;
            }else{
                VirtualInputManager.Instance.MoveUp=false;
            }
            if(Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_MOVE_DOWN])){
                VirtualInputManager.Instance.MoveDown=true;
            }else{
                VirtualInputManager.Instance.MoveDown=false;
            }
            if(Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_MOVE_RIGHT])){
                VirtualInputManager.Instance.MoveRight=true;
            }else{
                VirtualInputManager.Instance.MoveRight=false;
            }
            if(Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_MOVE_LEFT])){
                VirtualInputManager.Instance.MoveLeft=true;
            }else{
                VirtualInputManager.Instance.MoveLeft=false;
            }
            if(Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_JUMP])){
                VirtualInputManager.Instance.Jump=true;
            }else{
                VirtualInputManager.Instance.Jump=false;
            }
             if(Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_BLOCK])){
                VirtualInputManager.Instance.Block=true;
            }else{
                VirtualInputManager.Instance.Block=false;
            }
            if(Input.GetKey(VirtualInputManager.Instance.DicKeys[InputKeyType.KEY_ATTACK])) {
                //GetKeyDown是一种临时解决方案，我们可以使用索引器，或者回到空闲状态时将所有内容恢复为false
                //Idle中退出时将Attack恢复为false，目前解决了此问题
                VirtualInputManager.Instance.Attack = true;
            }
            else {
                VirtualInputManager.Instance.Attack = false;
            }
        }
    }

}
