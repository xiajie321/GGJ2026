using QFramework;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game
{
    public abstract class AbsControllerMono<T>: MonoBehaviour
    {
        protected Animator _animation;//动画控制
        protected Rigidbody2D _rigidbody2D;
        protected Collider2D _collider2D;
        protected Controller.Controller<T> _controller;
        protected FSM<T> _fsm = new();
        protected void InitComponents()
        {
            _animation ??= GetComponent<Animator>();
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
            _collider2D ??= GetComponent<Collider2D>();
        }
        public void SetController<TController>(TController controller)where TController : Controller<T>, new()//TODO 设置对应的控制器
        {
            _controller = controller;
            _controller.SetAnimation(_animation);
            _controller.SetRigidbody2D(_rigidbody2D);
            _controller.SetFsm(_fsm);
            _controller.SetCollider2D(_collider2D);
        }
    }
}