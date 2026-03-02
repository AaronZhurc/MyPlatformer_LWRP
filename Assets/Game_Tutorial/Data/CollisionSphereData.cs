using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games_tutorial
{
    [System.Serializable]
    public class CollisionSphereData
    {
        public List<GameObject> BottomSpheres;
        public List<GameObject> FrontSpheres=new List<GameObject>();
        public List<GameObject> BackSpheres=new List<GameObject>();
        public List<GameObject> UpSpheres=new List<GameObject>();
        
        public List<OverlapChecker> FrontOverlapCheckers=new List<OverlapChecker>();
        public List<OverlapChecker> AllOverlapCheckers=new List<OverlapChecker>();

        public delegate void DoSomething();
        public DoSomething Reposition_FrontSpheres;
        public DoSomething Reposition_BackSpheres;
        public DoSomething Reposition_BottomSpheres;
        public DoSomething Reposition_UpSpheres;
    }
}