using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class Settings : MonoBehaviour
    {
        public FrameSettings frameSettings;
        public PhysicsSettings physicsSettings;

        private void Awake(){
            //Frames
            Time.timeScale=frameSettings.TimeScale;
            Application.targetFrameRate=frameSettings.TargetFPS;

            //Physics
            Physics.defaultSolverIterations=physicsSettings.DefaultSolverIterations;
            Physics.defaultSolverVelocityIterations=physicsSettings.DefaultSolverVelocityIterations;
        
            //Default Keys
            VirtualInputManager.Instance.LoadKeys();
            //VirtualInputManager.Instance.SetDefaultKeys();
        }
    }
}