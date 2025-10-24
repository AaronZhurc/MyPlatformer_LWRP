using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Games_tutorial
{
    public class MouseControl : MonoBehaviour
    {
        Ray ray;
        RaycastHit hit;
        public PlayableCharacterType selectedCharacterType;
        public CharacterSelect characterSelect;
        CharacterSelectLight characterSelectLight;
        CharacterHoverLight characterHoverLight;
        Animator characterSelectCamAnimator;

        public void Awake(){
            characterSelect.SelectedCharacterType=PlayableCharacterType.NONE;
            characterSelectLight=GameObject.FindObjectOfType<CharacterSelectLight>();
            characterHoverLight=GameObject.FindAnyObjectByType<CharacterHoverLight>();

            characterSelectCamAnimator=GameObject.Find("CharacterSelectCameraController").GetComponent<Animator>();
        }

        private void Update(){
            ray=CameraManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit)){
                CharacterControl control=hit.collider.gameObject.GetComponent<CharacterControl>();
                if(control!=null){
                    selectedCharacterType=control.playableCharacterType;
                }else{
                    selectedCharacterType=PlayableCharacterType.NONE;
                }
            }
            if(Input.GetMouseButtonDown(0)){
                if(selectedCharacterType!=PlayableCharacterType.NONE){
                    characterSelect.SelectedCharacterType=selectedCharacterType;
                    characterSelectLight.transform.position=characterHoverLight.transform.position;
                    CharacterControl control=CharacterManager.Instance.GetCharacter(selectedCharacterType);
                    characterSelectLight.transform.parent=control.SkinnedMeshAnimator.transform;
                    characterSelectLight.selectLight.enabled=true;
                }else{
                    characterSelect.SelectedCharacterType=PlayableCharacterType.NONE;
                    characterSelectLight.selectLight.enabled=false;
                }
                foreach (CharacterControl c in CharacterManager.Instance.Characters)
                {
                    if(c.playableCharacterType==selectedCharacterType){
                        c.SkinnedMeshAnimator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.ClickAnimation],true);
                    }else{
                        c.SkinnedMeshAnimator.SetBool(HashManager.Instance.DicMainParams[TransitionParameter.ClickAnimation],false);
                    }
                }
                characterSelectCamAnimator.SetBool(selectedCharacterType.ToString(),true);
            }
        }
    }
}