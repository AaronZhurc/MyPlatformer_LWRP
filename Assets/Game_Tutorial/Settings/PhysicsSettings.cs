using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "PhysicsSettings", menuName = "Games/Settings/PhysicsSettings")]
    public class PhysicsSettings : ScriptableObject
    {
        public int DefaultSolverIterations;
        public int DefaultSolverVelocityIterations;
        
    }
}