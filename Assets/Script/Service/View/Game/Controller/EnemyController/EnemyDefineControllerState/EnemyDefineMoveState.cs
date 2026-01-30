using QFramework;

namespace Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState
{
    public class EnemyDefineMoveState:AbstractState<EnemyState,EnemyDefineControllerData>
    {
        public EnemyDefineMoveState(FSM<EnemyState> fsm, EnemyDefineControllerData owner) : base(fsm, owner)
        {
            
        }
        protected override void OnEnter()
        {
            mOwner.Animator.Play("Move");
        }

        protected override void OnUpdate()
        {
            
        }
    }
}