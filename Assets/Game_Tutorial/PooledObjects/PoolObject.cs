using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class PoolObject : MonoBehaviour
    {
        public PoolObjectType poolObjectType;
        public float ScheduledOffTime;
        private Coroutine OffRoutine; //我们想对关闭例程进行跟踪

        private void OnEnable(){ //启用时
            if(OffRoutine!=null){ //如果有前面的例程计划关闭
                StopCoroutine(OffRoutine); //我们希望停止，因为其现在有一个新的所有者，或者是一个新的攻击操作
            }
            if(ScheduledOffTime>0f){
                OffRoutine=StartCoroutine(_ScheduledOff()); //我们试图开始执行预定的例程，即执行关闭操作
            }
        }

        public void TurnOff(){
            this.transform.parent=null;
            this.transform.position=Vector3.zero;
            this.transform.rotation=Quaternion.identity;
            PoolManager.Instance.AddObject(this);
        }

        IEnumerator _ScheduledOff(){ //为每个池对象添加故障保护，此处创建一个预定的例程(预定关闭)
            yield return new WaitForSeconds(ScheduledOffTime);

            if(!PoolManager.Instance.PoolDictionary[poolObjectType].Contains(this.gameObject)){
                TurnOff();
            }
        }
    }
}