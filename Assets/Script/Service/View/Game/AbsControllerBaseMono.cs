using QFramework;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game
{
    public abstract class AbsControllerBaseMono<T>: MonoBehaviour
    {
        protected Animator _animation;//动画控制
        protected Rigidbody2D _rigidbody2D;
        protected Collider2D _collider2D;
        protected AbsControllerBase<T> AbsControllerBase;
        protected FSM<T> _fsm = new();
        protected void InitComponents()
        {
            _animation ??= GetComponent<Animator>();
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
            _collider2D ??= GetComponent<Collider2D>();
        }
        /// <summary>
        /// 更换控制模式,这里使用策略模式将同一种生物不同的控制方式解耦
        /// </summary>
        /// <param name="controller"></param>
        /// <typeparam name="TController"></typeparam>
        public void SetController<TController>(TController controller)where TController : AbsControllerBase<T>, new()//TODO 设置对应的控制器
        {
            AbsControllerBase = controller;
            AbsControllerBase.SetAnimation(_animation);
            AbsControllerBase.SetRigidbody2D(_rigidbody2D);
            AbsControllerBase.SetFsm(_fsm);
            AbsControllerBase.SetCollider2D(_collider2D);
        }
    }
}