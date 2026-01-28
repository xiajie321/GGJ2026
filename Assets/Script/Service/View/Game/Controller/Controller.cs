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
}