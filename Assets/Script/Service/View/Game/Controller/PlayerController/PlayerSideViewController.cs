using QFramework;
using Script.Service.View.Game.Controller.PlayerController.PlayerSideViewControllerState;
using UnityEngine;

namespace Script.Service.View.Game.Controller.PlayerController
{
    public class PlayerSideViewControllerData
    {
        public Animator Animator;
        public Rigidbody2D Rigidbody2D;
        public Collider2D Collider2D;
    }
    /// <summary>
    /// 侧面观察模式控制器
    /// </summary>
    public class PlayerSideViewController:Controller
    {
        protected override void Init(FSM<State> fsm)
        {
            var ls = new PlayerSideViewControllerData()
            {
                Animator = _animation,
                Rigidbody2D = _rigidbody2D,
                Collider2D = _collider2D,
            };
            fsm.AddState(State.Idle,new PlayerSideViewIdle(fsm,ls));
            //TODO 后续状态往这里添加
            fsm.StartState(State.Idle);
        }
    }
}