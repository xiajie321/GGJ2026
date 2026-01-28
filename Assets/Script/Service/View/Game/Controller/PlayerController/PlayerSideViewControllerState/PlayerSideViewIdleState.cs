using QFramework;

namespace Script.Service.View.Game.Controller.PlayerController.PlayerSideViewControllerState
{
    public class PlayerSideViewIdleState:AbstractState<PlayerState,PlayerSideViewControllerData>
    {
        public PlayerSideViewIdleState(FSM<PlayerState> fsm, PlayerSideViewControllerData owner) : base(fsm, owner)
        {
        }
    }
}