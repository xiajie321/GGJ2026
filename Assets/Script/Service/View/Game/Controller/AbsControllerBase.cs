using QFramework;
using Script.Service.Architecture;
using Script.Service.View.Game.Component;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Service.View.Game.Controller
{
    public abstract class AbsControllerBase:MonoBehaviour,IController
    {
        /// <summary>
        /// 用于控制血条显示
        /// </summary>
        protected HealthBarComponent HealthBarComponent;
        /// <summary>
        /// 用于切换动画控制器
        /// </summary>
        protected Animator Animator;
        /// <summary>
        /// 变更生命值执行的方法
        /// </summary>
        public abstract int Hp { get; set; }
        /// <summary>
        /// 变更速度执行的方法
        /// </summary>
        public abstract int Speed { get; set; }
        /// <summary>
        /// 被伤害时执行的方法
        /// </summary>
        public abstract void Harm(HarmData data);

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }

    public struct HarmData
    {
        public int Hp;

        public int Damage { get; internal set; }
        public EnemyController Attacker { get; internal set; }
    }
}