using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class PlayGameButton : MonoBehaviour
    {
        public void OnClick_PlayGame(){
            // Debug.Log("clicked:playgame");
            UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialScene_Sample_Day");
        }
    }
}