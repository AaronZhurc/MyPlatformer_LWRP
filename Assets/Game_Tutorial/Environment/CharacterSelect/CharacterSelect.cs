using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public enum PlayableCharacterType{
        NONE,
        WHITE,
        BLACK,
    }

    [CreateAssetMenu(fileName = "CharacterSelect", menuName = "Games/CharacterSelect/CharacterSelect")]
    public class CharacterSelect : ScriptableObject
    {
        public PlayableCharacterType SelectedCharacterType;
    }
}