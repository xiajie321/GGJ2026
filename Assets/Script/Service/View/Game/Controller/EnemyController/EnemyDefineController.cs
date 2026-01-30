using QFramework;
using Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState;
using UnityEngine;

namespace Script.Service.View.Game.Controller.EnemyController
{
    public class EnemyDefineControllerData
    {
        public Animator Animator;
        public Rigidbody2D Rigidbody2D;
        public Collider2D Collider2D;
    }
    public class EnemyDefineController:AbsControllerBase<EnemyState>
    {
        protected override void Init(FSM<EnemyState> fsm)
        {
            var ls = new EnemyDefineControllerData()
            {
                Animator = _animation,
                Rigidbody2D = _rigidbody2D,
                Collider2D = _collider2D,
            };
            fsm.AddState(EnemyState.Idle,new EnemyDefineIdleState(fsm,ls));
            fsm.AddState(EnemyState.Move,new EnemyDefineMoveState(fsm,ls));
            fsm.AddState(EnemyState.Interaction,new EnemyDefineInteractionState(fsm,ls));
            fsm.StartState(EnemyState.Idle);
        }
    }
}