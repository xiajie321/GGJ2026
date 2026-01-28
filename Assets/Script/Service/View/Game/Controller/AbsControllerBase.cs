using QFramework;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public abstract class AbsControllerBase<T>
    {
        protected Rigidbody2D _rigidbody2D;
        protected Animator _animation;
        protected Collider2D _collider2D;
        protected FSM<T> _fsm;
        internal void SetAnimation(Animator animation)
        {
            _animation = animation;
        }
        internal void SetFsm(FSM<T> fsm)
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

        protected abstract void Init(FSM<T> fsm);
    }
}