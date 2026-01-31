using QFramework;
using UnityEngine;

namespace Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState
{
    public class EnemyDefineMoveState:AbstractState<EnemyState,EnemyDefineControllerData>
    {
        public EnemyDefineMoveState(FSM<EnemyState> fsm, EnemyDefineControllerData owner) : base(fsm, owner)
        {
            
        }
        private float _time;
        protected override void OnEnter()
        {
            mOwner.Animator.Play("Walk");
            _time = 0f;
        }

        protected override void OnFixedUpdate()
        {
            if (Random.Range(0, 1000) == 0)
            {
                mFSM.ChangeState(EnemyState.Idle);
            }
        }

        protected override void OnUpdate()
        {
            float speed = mOwner.EnemyData.MoveSpeed;
            if (mOwner.IsAngry)
            {
                speed *= 2;
            }
            mOwner.Rigidbody2D.velocity = new Vector2(speed, 0);
            
            if (mOwner.IsAngry) return;
            
            if (_time < mOwner.EnemyData.ThinkCoolingTime)
            {
                _time += Time.deltaTime;
                return;
            }
            if (mOwner.EnemyTriggerMono.TrapControllerMonos.Count != 0)
            {
                mFSM.ChangeState(EnemyState.Interaction);
            }
        }
    }
}
