using QFramework;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game.FSMState.PlayerControllerState
{
    public class IdleState : AbstractPlayerState
    {
        public IdleState(FSM<PlayerStateEnum> fsm, PlayerController target) : base(fsm, target)
        {
        }

        protected override void OnEnter()
        {
            // 停止水平移动
            if (mController.Rigidbody != null)
            {
                Vector2 velocity = mController.Rigidbody.velocity;
                velocity.x = 0;
                mController.Rigidbody.velocity = velocity;
            }
            // 播放待机动画
            mController.GetAnimator().Play("Idle");
        }

        protected override void OnUpdate()
        {
            FaceMouse();

            var config = mController.GameConfig.GameConfig.PlayerConfig;

            if (Input.GetKey(config.MoveLeftKey) || Input.GetKey(config.MoveRightKey))
            {
                mFSM.ChangeState(PlayerStateEnum.Move);
            }
            else if (Input.GetKey(config.JumpKey) && mController.IsGrounded)
            {
                mFSM.ChangeState(PlayerStateEnum.Jump);
            }
            else if (Input.GetKeyDown(config.AttackKey))
            {
                mFSM.ChangeState(PlayerStateEnum.Attack);
            }
        }
    }
}
