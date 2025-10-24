using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Games_tutorial
{
    [CustomEditor(typeof(ColliderRemover))]
    public class ColliderRemoverEditor : Editor
    {
        override public void OnInspectorGUI(){
            DrawDefaultInspector();

            if(GUILayout.Button("Remove All Colliders")){
                ColliderRemover rem=target as ColliderRemover;
                rem.RemoveAllColliders();
            }
        }
    }
}