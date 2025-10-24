using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [CreateAssetMenu(fileName = "SavedKeys", menuName = "Games/Settings/SavedKeys")]
    public class SavedKeys : ScriptableObject
    {
        public List<KeyCode> KeyCodesList=new List<KeyCode>();
        
    }
}

