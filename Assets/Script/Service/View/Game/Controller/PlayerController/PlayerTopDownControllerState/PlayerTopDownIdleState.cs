using QFramework;

namespace Script.Service.View.Game.Controller.PlayerController.PlayerTopDownControllerState
{
    /// <summary>
    /// 俯视角下的闲置状态
    /// </summary>
    public class PlayerTopDownIdleState:AbstractState<PlayerState,PlayerTopDownControllerData>
    {
        public PlayerTopDownIdleState(FSM<PlayerState> fsm, PlayerTopDownControllerData owner) : base(fsm, owner)
        {
        }
    }
}