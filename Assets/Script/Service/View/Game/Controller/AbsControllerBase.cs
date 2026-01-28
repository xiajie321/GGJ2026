using QFramework;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    /// <summary>
    /// 逻辑控制器的抽象基类，用于解耦 MonoBehaviour 和具体的控制逻辑
    /// </summary>
    /// <typeparam name="T">状态枚举类型</typeparam>
    public abstract class AbsControllerBase<T>
    {
        protected Rigidbody2D _rigidbody2D;
        protected Animator _animation;
        protected Collider2D _collider2D;
        protected FSM<T> _fsm;

        /// <summary>
        /// 设置动画组件
        /// </summary>
        /// <param name="animation"></param>
        internal void SetAnimation(Animator animation)
        {
            _animation = animation;
        }

        /// <summary>
        /// 设置并初始化状态机
        /// </summary>
        /// <param name="fsm"></param>
        internal void SetFsm(FSM<T> fsm)
        {
            _fsm?.Clear();
            _fsm = fsm;
            Init(fsm);
        }

        /// <summary>
        /// 设置刚体组件
        /// </summary>
        /// <param name="rigidbody2D"></param>
        internal void SetRigidbody2D(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
        }

        /// <summary>
        /// 设置碰撞体组件
        /// </summary>
        /// <param name="collider2D"></param>
        internal void SetCollider2D(Collider2D collider2D)
        {
            _collider2D = collider2D;
        }

        /// <summary>
        /// 初始化控制器，子类在此处添加状态和启动状态机
        /// </summary>
        /// <param name="fsm"></param>
        protected abstract void Init(FSM<T> fsm);
    }
}