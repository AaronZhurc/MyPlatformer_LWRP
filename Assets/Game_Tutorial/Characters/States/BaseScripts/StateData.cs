using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games_tutorial
{
    public abstract class StateData : ScriptableObject //无需实例化，数据容器。本类将会成为我们所有预定义操作的基类
    {
        public abstract void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo);
        public abstract void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo);
        public abstract void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo);
    }
}