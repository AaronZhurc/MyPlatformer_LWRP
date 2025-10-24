using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

namespace Games_tutorial
{
    public class PlayGame : MonoBehaviour
    {
        public CharacterSelect characterSelect;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return)){
                if(characterSelect.SelectedCharacterType!=PlayableCharacterType.NONE){
                    UnityEngine.SceneManagement.SceneManager.LoadScene(RBScenes.TutorialScene_Sample.ToString());
                }else{
                    Debug.Log("must select character first");
                }
            }
        }
    }
}