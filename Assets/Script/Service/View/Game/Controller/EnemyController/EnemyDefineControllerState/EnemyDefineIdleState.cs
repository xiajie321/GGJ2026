using QFramework;

namespace Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState
{
    public class EnemyDefineIdleState:AbstractState<EnemyState,EnemyDefineControllerData>
    {
        public EnemyDefineIdleState(FSM<EnemyState> fsm, EnemyDefineControllerData owner) : base(fsm, owner)
        {
        }

        protected override void OnEnter()
        {
            mOwner.Animator.Play("Idle");
        }

        protected override void OnUpdate()
        {
        }
    }
}