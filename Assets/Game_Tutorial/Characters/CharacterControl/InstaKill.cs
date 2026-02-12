using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class InstaKill : MonoBehaviour
    {
        CharacterControl control;
        private void Start() {
            control=this.gameObject.GetComponentInParent<CharacterControl>();
        }

        private void FixedUpdate() {
            if(control.subComponentProcessor.ComponentsDic.ContainsKey(SubComponentType.MANUAL_INPUT)) {
                return;
            }
            if(!control.SkinnedMeshAnimator.GetBool(HashManager.Instance.DicMainParams[TransitionParameter.Grounded])) {
                return;
            }
            
            foreach (KeyValuePair<TriggerDetector,List<Collider>> data in control.animationProgress.CollidingBodyParts) {
                foreach(Collider col in data.Value) {
                    CharacterControl c=CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);

                    if(c == control) {
                        continue;
                    }

                    if(!control.subComponentProcessor.ComponentsDic.ContainsKey(SubComponentType.MANUAL_INPUT)) {
                        continue;
                    }

                    if(!c.SkinnedMeshAnimator.GetBool(HashManager.Instance.DicMainParams[TransitionParameter.Grounded])) {
                        continue;
                    }

                    if(c.animationProgress.IsRunning(typeof(Attack))){
                        continue;
                    }

                    if(control.animationProgress.IsRunning(typeof(Attack))) {
                        continue;
                    }

                    if(c.animationProgress.StateNameContains("RunningSlide")) {
                        continue;
                    }

                    if(c.DAMAGE_DATA.IsDead()) {
                        continue;
                    }

                    if(control.DAMAGE_DATA.IsDead()) {
                        continue;
                    }

                    Debug.Log("insta kill");
                    c.damageDetector.DeathByInstaKill(control);

                    return;
                }
            }
        }
    }
}