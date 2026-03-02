using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "ToggleBoxCollider", menuName = "Games/AbilityData/ToggleBoxCollider")]
    public class ToggleBoxCollider : StateData
    {
        public bool On;
        public bool OnStart;
        public bool OnEnd;

        [Space(10)]
        public bool RepositionSpheres;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(OnStart){
                // CharacterControl control=characterState.GetCharacterControl(animator);
                CharacterControl control=characterState.characterControl;
                ToggleBoxCol(control);
            }
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(OnEnd){
                // CharacterControl control=characterState.GetCharacterControl(animator);
                CharacterControl control=characterState.characterControl;
                ToggleBoxCol(control);
            }
        }
        private void ToggleBoxCol(CharacterControl control){
            control.RIGID_BODY.velocity=Vector3.zero;
            control.GetComponent<BoxCollider>().enabled=On;

            if(RepositionSpheres){
                control.COLLISION_SPHERE_DATA.Reposition_FrontSpheres();
                control.COLLISION_SPHERE_DATA.Reposition_BackSpheres();
                control.COLLISION_SPHERE_DATA.Reposition_BottomSpheres();
                control.COLLISION_SPHERE_DATA.Reposition_UpSpheres();
            }
        }
    }
}
