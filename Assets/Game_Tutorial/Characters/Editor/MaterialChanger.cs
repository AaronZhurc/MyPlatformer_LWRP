using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Games_tutorial
{
    [CustomEditor(typeof(CharacterControl))]
    public class MaterialChanger : Editor
    {
        public override void OnInspectorGUI() //添加一个按钮
        {
            DrawDefaultInspector();
            CharacterControl control=(CharacterControl)target;
            if(GUILayout.Button("Change Material")){
                control.ChangeMaterial();
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}