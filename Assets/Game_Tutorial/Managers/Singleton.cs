using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public class Singleton<T> : MonoBehaviour where T: MonoBehaviour //泛化
    {
        //假设我们有ABC三个角色，还有一个角色管理器。游戏开始时角色都回去找管理器进行注册，此时管理器会在整个游戏中拥有一个场景中所有角色的完整列表
        //此时我们可以通过该管理器找到角色，或查明某个对象是否为一个角色  
        //虚拟输入管理器也有类似的情形(对操作)
        private static T _instance; 
        public static T Instance{ //链接到游戏中的对象
            get{
                //_instance=(T)FindObjectOfType(typeof(T)); 
                //当我们第一次运行此代码，实例永远为空，在尝试获取实例时都执行查找操作只会降低性能
                //如果我们拥有能够查找多实例并将其删除的代码就好，但这是一个较为简单的游戏
                //只需记住不要将任何单例放入任何游戏对象中就行
                if(_instance==null){ //只要任何脚本尝试访问，如果没有实例，就会自动建立一个
                    GameObject obj=new GameObject(); 
                    _instance=obj.AddComponent<T>(); //添加组件
                    obj.name=typeof(T).ToString();
                }
                return _instance;
            }
        }
        
    }
}