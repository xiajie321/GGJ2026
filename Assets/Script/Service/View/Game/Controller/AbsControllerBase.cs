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
        /// 临时速度(生物速度 = 配表速度 + 临时速度)
        /// </summary>
        protected float TemporarySpeed;
        /// <summary>
        /// 临时生命(生物生命 = 配表生命 + 临时生命)
        /// </summary>
        private int TemporaryHp;
        /// <summary>
        /// 配表的速度
        /// </summary>
        public abstract float Speed { get; }
        /// <summary>
        /// 配表的生命值
        /// </summary>
        public abstract int Hp { get; }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}