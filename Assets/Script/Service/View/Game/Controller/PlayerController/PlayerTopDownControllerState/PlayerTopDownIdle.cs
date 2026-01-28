using QFramework;

namespace Script.Service.View.Game.Controller.PlayerController.PlayerTopDownControllerState
{
    public class PlayerTopDownIdle:AbstractState<State,PlayerTopDownControllerData>
    {
        public PlayerTopDownIdle(FSM<State> fsm, PlayerTopDownControllerData owner) : base(fsm, owner)
        {
        }
    }
}