using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class CharacterSelectLight : MonoBehaviour
    {
        public Light selectLight;
        private void Start(){
            selectLight=GetComponent<Light>();
            selectLight.enabled=false;
        }
    }
}