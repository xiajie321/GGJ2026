using QFramework;

namespace Script.Service.View.Game.Controller.PlayerController.PlayerSideViewControllerState
{
    public class PlayerSideViewIdle:AbstractState<State,PlayerSideViewControllerData>
    {
        public PlayerSideViewIdle(FSM<State> fsm, PlayerSideViewControllerData owner) : base(fsm, owner)
        {
            
        }
    }
}