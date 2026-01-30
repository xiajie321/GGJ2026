using Alchemy.Serialization;
using QFramework;
using Script.Service.Architecture;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game
{
    /// <summary>
    /// 游戏对象控制器的 MonoBehaviour 基类
    /// </summary>
    /// <typeparam name="T">状态枚举类型</typeparam>
    [AlchemySerialize]
    public abstract partial class AbsControllerBaseMono<T>: MonoBehaviour,IController
    {
        /// <summary>
        /// 动画组件
        /// </summary>
        protected Animator _animation;

        /// <summary>
        /// 2D 刚体组件
        /// </summary>
        protected Rigidbody2D _rigidbody2D;

        /// <summary>
        /// 2D 碰撞体组件
        /// </summary>
        protected Collider2D _collider2D;

        /// <summary>
        /// 逻辑控制器基类实例
        /// </summary>
        protected AbsControllerBase<T> AbsControllerBase;

        /// <summary>
        /// 状态机实例
        /// </summary>
        protected FSM<T> _fsm = new();

        /// <summary>
        /// 初始化组件
        /// </summary>
        protected void InitComponents()
        {
            _animation ??= GetComponent<Animator>();
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
            _collider2D ??= GetComponent<Collider2D>();
        }

        /// <summary>
        /// 更换控制模式,这里使用策略模式将同一种生物不同的控制方式解耦
        /// </summary>
        /// <param name="controller">要设置的逻辑控制器实例</param>
        /// <typeparam name="TController">控制器类型</typeparam>
        public void SetController<TController>(TController controller)where TController : AbsControllerBase<T>, new()
        {
            _fsm?.Clear();
            AbsControllerBase = controller;
            AbsControllerBase.SetAnimation(_animation);
            AbsControllerBase.SetRigidbody2D(_rigidbody2D);
            AbsControllerBase.SetFsm(_fsm);
            AbsControllerBase.SetCollider2D(_collider2D);
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}