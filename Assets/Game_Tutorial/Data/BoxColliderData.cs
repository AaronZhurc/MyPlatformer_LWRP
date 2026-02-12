using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [System.Serializable]
    public class BoxColliderData
    {
        // public bool UpdatingBoxCollider;
        public bool IsUpdatingSpheres;
        public bool IsLanding;
        
        public float Size_Update_Speed;
        public float Center_Update_Speed;

        public Vector3 TargetSize;
        public Vector3 TargetCenter;
        public Vector3 LandingPosition;
    }
}