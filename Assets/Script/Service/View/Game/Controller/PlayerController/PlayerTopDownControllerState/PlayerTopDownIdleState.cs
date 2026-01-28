using QFramework;

namespace Script.Service.View.Game.Controller.PlayerController.PlayerTopDownControllerState
{
    public class PlayerTopDownIdleState:AbstractState<PlayerState,PlayerTopDownControllerData>
    {
        public PlayerTopDownIdleState(FSM<PlayerState> fsm, PlayerTopDownControllerData owner) : base(fsm, owner)
        {
        }
    }
}