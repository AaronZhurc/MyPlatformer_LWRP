using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "FrameSettings", menuName = "Games/Settings/FrameSettings")]
    public class FrameSettings : ScriptableObject
    {
        [Range(0.01f,1f)]
        public float TimeScale;
        public int TargetFPS;
    }
}