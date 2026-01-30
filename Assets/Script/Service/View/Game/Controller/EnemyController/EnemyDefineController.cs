using QFramework;
using Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.View.Game.Controller.EnemyController
{
    public class EnemyDefineControllerData
    {
        public Animator Animator;
        public Rigidbody2D Rigidbody2D;
        public Collider2D Collider2D;
        public EnemyData EnemyData;
    }
    public class EnemyDefineController:AbsControllerBase<EnemyState>
    {
        private EnemyData _enemyData;
        protected override void Init(FSM<EnemyState> fsm)
        {
            var ls = new EnemyDefineControllerData()
            {
                Animator = _animation,
                Rigidbody2D = _rigidbody2D,
                Collider2D = _collider2D,
                EnemyData = _enemyData,
            };
            fsm.AddState(EnemyState.Idle,new EnemyDefineIdleState(fsm,ls));
            fsm.AddState(EnemyState.Move,new EnemyDefineMoveState(fsm,ls));
            fsm.AddState(EnemyState.Interaction,new EnemyDefineInteractionState(fsm,ls));
            fsm.StartState(EnemyState.Idle);
        }

        public void SetEnemyData(EnemyData data)
        {
            _enemyData = data;
        }
    }
}