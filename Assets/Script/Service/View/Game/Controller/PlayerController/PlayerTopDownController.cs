using QFramework;
using Script.Service.View.Game.Controller.PlayerController.PlayerTopDownControllerState;
using UnityEngine;

namespace Script.Service.View.Game.Controller.PlayerController
{
    /// <summary>
    /// 俯视角控制器所需的数据
    /// </summary>
    public class PlayerTopDownControllerData
    {
        public Animator Animator;
        public Rigidbody2D Rigidbody2D;
        public Collider2D Collider2D;
    }
    /// <summary>
    /// 俯视角控制器，负责管理俯视角下的玩家行为状态
    /// </summary>
    public class PlayerTopDownController:AbsControllerBase<PlayerState>
    {
        protected override void Init(FSM<PlayerState> fsm)
        {
            var ls = new PlayerTopDownControllerData()
            {
                Animator = _animation,
                Rigidbody2D = _rigidbody2D,
                Collider2D = _collider2D,
            };
            fsm.AddState(PlayerState.Idle,new PlayerTopDownIdleState(fsm,ls));
            //TODO 后续状态往这里添加
            fsm.StartState(PlayerState.Idle);
            Debug.Log("[PlayerTopDownController] 加载完成");
        }
    }
}