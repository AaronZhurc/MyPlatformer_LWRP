using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    [System.Serializable]
    public class ManualInputData
    {
        public delegate bool ReturnBool();

        public ReturnBool DoubleTapUp;
        public ReturnBool DoubleTapDown;
    }
}