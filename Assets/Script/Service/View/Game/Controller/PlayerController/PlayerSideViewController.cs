using QFramework;
using Script.Service.View.Game.Controller.PlayerController.PlayerSideViewControllerState;
using UnityEngine;

namespace Script.Service.View.Game.Controller.PlayerController
{
    /// <summary>
    /// 侧面观察模式控制器所需的数据
    /// </summary>
    public class PlayerSideViewControllerData
    {
        public Animator Animator;
        public Rigidbody2D Rigidbody2D;
        public Collider2D Collider2D;
    }
    /// <summary>
    /// 侧面观察模式控制器，负责管理侧视角下的玩家行为状态
    /// </summary>
    public class PlayerSideViewController:AbsControllerBase<PlayerState>
    {
        protected override void Init(FSM<PlayerState> fsm)
        {
            var ls = new PlayerSideViewControllerData()
            {
                Animator = _animation,
                Rigidbody2D = _rigidbody2D,
                Collider2D = _collider2D,
            };
            fsm.AddState(PlayerState.Idle,new PlayerSideViewIdleState(fsm,ls));
            //TODO 后续状态往这里添加
            fsm.StartState(PlayerState.Idle);
            Debug.Log("[PlayerSideViewController] 加载完成");
        }
    }
}