using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace Games_tutorial
{
    public enum UIParameters{
        ScaleUp,
    }
    public class ButtonScale : MonoBehaviour
    {
        GameEventListener listener;
        Dictionary<GameObject,Animator> DicButtons = new Dictionary<GameObject,Animator>();

        private void Awake()
        {
            listener=GetComponent<GameEventListener>();

        }

        public void ScaleUpBotton(){
            GetButtonAnimator(listener.gameEvent.EVENTOBJ).SetBool(UIParameters.ScaleUp.ToString(), true);
            //listener.gameEvent.EVENTOBJ.GetComponent<Animator>().SetBool(UIParameters.ScaleUp.ToString(), true);
        }

        public void ResetBottonScale(){
            GetButtonAnimator(listener.gameEvent.EVENTOBJ).SetBool(UIParameters.ScaleUp.ToString(), false);
            //listener.gameEvent.EVENTOBJ.GetComponent<Animator>().SetBool(UIParameters.ScaleUp.ToString(), false);
        }

        Animator GetButtonAnimator(GameObject obj){
            if(!DicButtons.ContainsKey(obj)){
                Animator animator=obj.GetComponent<Animator>();
                DicButtons.Add(obj,animator);
                return animator;
            }else{
                return DicButtons[obj];
            }
        }
    }
}