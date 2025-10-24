using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class CharacterHoverLight : MonoBehaviour
    {
        //因为目前的代码(不知为何)能够以及能够做到我们想要的结果，因此白色圆圈标记不会被使用，可以查看课程#35
        public Vector3 Offset=new Vector3();

        CharacterControl HoverSelectedCharacter;
        MouseControl mouseHoverSelect;
        Vector3 TargetPos=new Vector3();
        Light hoverLight; 

        private void Start(){
            mouseHoverSelect=GameObject.FindObjectOfType<MouseControl>();
            hoverLight=GetComponent<Light>();
        }
        private void Update(){
            if(mouseHoverSelect.selectedCharacterType==PlayableCharacterType.NONE){
                HoverSelectedCharacter=null;
                hoverLight.enabled=false;
            }else{
                hoverLight.enabled=true;
                LightUpSelectedCharacter();
            }
        }
        private void LightUpSelectedCharacter(){
            if(HoverSelectedCharacter==null){
                HoverSelectedCharacter=CharacterManager.Instance.GetCharacter(mouseHoverSelect.selectedCharacterType);
                this.transform.position=HoverSelectedCharacter.SkinnedMeshAnimator.transform.position+HoverSelectedCharacter.transform.TransformDirection(Offset);
                this.transform.parent=HoverSelectedCharacter.SkinnedMeshAnimator.transform;
            }
        }
    }
}