using QFramework;
using Script.Service.View.Game.Controller.PlayerController.PlayerTopDownControllerState;
using UnityEngine;

namespace Script.Service.View.Game.Controller.PlayerController
{
    public class PlayerTopDownControllerData
    {
        public Animator Animator;
        public Rigidbody2D Rigidbody2D;
        public Collider2D Collider2D;
    }
    /// <summary>
    /// 俯视角控制器
    /// </summary>
    public class PlayerTopDownController:Controller
    {
        protected override void Init(FSM<State> fsm)
        {
            var ls = new PlayerTopDownControllerData()
            {
                Animator = _animation,
                Rigidbody2D = _rigidbody2D,
                Collider2D = _collider2D,
            };
            fsm.AddState(State.Idle,new PlayerTopDownIdle(fsm,ls));
            //TODO 后续状态往这里添加
            fsm.StartState(State.Idle);
        }
    }
}