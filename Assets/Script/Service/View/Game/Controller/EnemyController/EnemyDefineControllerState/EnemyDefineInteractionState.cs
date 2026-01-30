using QFramework;

namespace Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState
{
    public class EnemyDefineInteractionState:AbstractState<EnemyState,EnemyDefineControllerData>
    {
        public EnemyDefineInteractionState(FSM<EnemyState> fsm, EnemyDefineControllerData owner) : base(fsm, owner)
        {
            
        }
        protected override void OnEnter()
        {
            mOwner.Animator.Play("Interaction");
        }

        protected override void OnUpdate()
        {
            
        }
    }
}