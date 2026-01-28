using QFramework;

namespace Script.Service.View.Game.Controller.PlayerController.PlayerSideViewControllerState
{
    public class PlayerSideViewIdle:AbstractState<PlayerState,PlayerSideViewControllerData>
    {
        public PlayerSideViewIdle(FSM<PlayerState> fsm, PlayerSideViewControllerData owner) : base(fsm, owner)
        {
            
        }
    }
}