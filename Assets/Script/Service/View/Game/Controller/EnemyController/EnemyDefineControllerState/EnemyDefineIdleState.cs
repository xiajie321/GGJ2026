using QFramework;
using UnityEngine;

namespace Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState
{
    public class EnemyDefineIdleState:AbstractState<EnemyState,EnemyDefineControllerData>
    {
        public EnemyDefineIdleState(FSM<EnemyState> fsm, EnemyDefineControllerData owner) : base(fsm, owner)
        {
        }

        private float _time;
        protected override void OnEnter()
        {
            mOwner.Animator.Play("Idle");
            _time = 0;
        }
        
        protected override void OnUpdate()
        {
            _time += UnityEngine.Time.deltaTime;
            if (_time >= mOwner.EnemyData.StandTime)//超过时间就会切换移动状态
            {
                mFSM.ChangeState(EnemyState.Move);
            }
        }
        
    }
}