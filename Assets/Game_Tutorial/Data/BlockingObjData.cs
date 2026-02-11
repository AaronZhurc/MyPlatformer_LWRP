using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    [System.Serializable]
    public class BlockingObjData : MonoBehaviour
    {
        public int FrontBlockingDicCount;
        public int UpBlockingDicCount;

        public delegate void DoSomething();
        public delegate bool ReturnBool();
        public delegate List<GameObject> ReturnGameObjectList();
        public DoSomething ClearFrontBlockingObjDic;
        public ReturnBool LeftSideBlocked;
        public ReturnBool RightSideBlocked;
        public ReturnGameObjectList GetFrontBlockingObjList;
        public ReturnGameObjectList GetFrontBlockingCharacterList;
    }
}