using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    [System.Serializable]
    public class LedgeGrabData
    {
        public bool isGrabbingLedge;
        // public Ledge GrabbedLedge;

        public delegate void DoSomething();
        public DoSomething LedgeCollidersOff;
    }
}