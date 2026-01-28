using QFramework;

namespace Script.Service.View.Game.Controller.PlayerController.PlayerTopDownControllerState
{
    public class PlayerTopDownIdle:AbstractState<PlayerState,PlayerTopDownControllerData>
    {
        public PlayerTopDownIdle(FSM<PlayerState> fsm, PlayerTopDownControllerData owner) : base(fsm, owner)
        {
        }
    }
}