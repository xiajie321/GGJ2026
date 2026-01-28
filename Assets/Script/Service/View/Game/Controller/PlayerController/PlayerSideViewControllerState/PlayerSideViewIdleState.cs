using QFramework;

namespace Script.Service.View.Game.Controller.PlayerController.PlayerSideViewControllerState
{
    /// <summary>
    /// 侧视角下的闲置状态
    /// </summary>
    public class PlayerSideViewIdleState:AbstractState<PlayerState,PlayerSideViewControllerData>
    {
        public PlayerSideViewIdleState(FSM<PlayerState> fsm, PlayerSideViewControllerData owner) : base(fsm, owner)
        {
        }
    }
}