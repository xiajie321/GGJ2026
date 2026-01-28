using QFramework;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public abstract class Controller
    {
        protected Rigidbody2D _rigidbody2D;
        protected Animator _animation;
        protected Collider2D _collider2D;
        protected FSM<State> _fsm;
        internal void SetAnimation(Animator animation)
        {
            _animation = animation;
        }
        internal void SetFsm(FSM<State> fsm)
        {
            _fsm?.Clear();
            _fsm = fsm;
            Init(fsm);
        }
        internal void SetRigidbody2D(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
        }

        internal void SetCollider2D(Collider2D collider2D)
        {
            _collider2D = collider2D;
        }

        protected abstract void Init(FSM<State> fsm);
    }
    public enum State//TODO 如有需要可以在这里补充状态
    {
        /// <summary>
        /// 静止状态
        /// </summary>
        Idle,
        /// <summary>
        /// 移动状态
        /// </summary>
        Move,
        /// <summary>
        /// 跳跃状态
        /// </summary>
        Jump,
        /// <summary>
        /// 下落状态
        /// </summary>
        Whereabouts,
        /// <summary>
        /// 攻击状态
        /// </summary>
        Attack,
        /// <summary>
        /// 无敌状态
        /// </summary>
        Invincible,
        /// <summary>
        /// 死亡状态
        /// </summary>
        Dead,
        /// <summary>
        /// 攻击状态
        /// </summary>
        Injured,
    }
}