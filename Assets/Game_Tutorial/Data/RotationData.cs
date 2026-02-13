using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    [System.Serializable]

    public class RotationData 
    {
        public bool LockEarlyTurn;
        public bool LockDirectionNextState;
        public delegate bool ReturnBool();
        public delegate void DoSomthing(bool faceForward);
        public ReturnBool EarlyTurnIsLocked;
        public ReturnBool IsFacingForward;
        public DoSomthing FaceForward;
    }
}