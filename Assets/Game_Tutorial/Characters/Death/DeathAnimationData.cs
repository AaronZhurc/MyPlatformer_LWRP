using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public enum DeathType{
        None,
        LAUNCH_INTO_AIR,
        GROUND_SHOCK
    }
    [CreateAssetMenu(fileName = "New ScriptableObject", menuName = "Games/Death/DeathAnimationData")]
    public class DeathAnimationData : ScriptableObject
    {
        // public List<GeneralBodyPart> GeneralBodyParts=new List<GeneralBodyPart>();
        public RuntimeAnimatorController Animator;
        public DeathType deathType;
        public bool IsFacingAttacker;
    }
}