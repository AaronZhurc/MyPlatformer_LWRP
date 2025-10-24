using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Games_tutorial
{
    public class ChangeScene : MonoBehaviour
    {
        public string nextScene;

        public void ChangeSceneTo(){
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);  
        }
    }
}