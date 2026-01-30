using QFramework;

namespace Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState
{
    public class EnemyDefineMoveState:AbstractState<EnemyState,EnemyDefineControllerData>
    {
        public EnemyDefineMoveState(FSM<EnemyState> fsm, EnemyDefineControllerData owner) : base(fsm, owner)
        {
            
        }
    }
}