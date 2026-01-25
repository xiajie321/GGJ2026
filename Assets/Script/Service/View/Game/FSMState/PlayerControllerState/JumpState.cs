using QFramework;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game.FSMState.PlayerControllerState
{
    public class JumpState : AbstractPlayerState
    {
        public JumpState(FSM<PlayerStateEnum> fsm, PlayerController target) : base(fsm, target)
        {
        }

        protected override void OnEnter()
        {
            // 施加向上的力
            // 跳跃高度 = v^2 / (2g)  => v = sqrt(2 * g * h)
            // Physics2D.gravity.y 是负值，需要取绝对值或者乘 -1
            
            if (mController.Rigidbody != null)
            {
                float jumpHeight = mController.GameConfig.GameConfig.PlayerConfig.JumpHeight;
                float gravity = Mathf.Abs(Physics2D.gravity.y * mController.Rigidbody.gravityScale);
                float jumpSpeed = Mathf.Sqrt(2 * gravity * jumpHeight);

                Vector2 velocity = mController.Rigidbody.velocity;
                velocity.y = jumpSpeed;
                mController.Rigidbody.velocity = velocity;
            }
            
            // 播放跳跃动画
            // TODO mController.Animator.Play("Jump");
        }

        protected override void OnUpdate()
        {
            FaceMouse();
            
            // 空中移动控制（如果允许）
            var config = mController.GameConfig.GameConfig.PlayerConfig;
            float moveX = 0;

            if (Input.GetKey(config.MoveLeftKey))
            {
                // 墙壁检测
                if (!mController.IsTouchingLeftWall)
                {
                    moveX = -1;
                }
            }
            else if (Input.GetKey(config.MoveRightKey))
            {
                // 墙壁检测
                if (!mController.IsTouchingRightWall)
                {
                    moveX = 1;
                }
            }

            if (mController.Rigidbody != null)
            {
                Vector2 velocity = mController.Rigidbody.velocity;
                velocity.x = moveX * mController.Speed;
                mController.Rigidbody.velocity = velocity;
            }

            // 落地检测：使用 IsGrounded 以及向下的速度
            if (mController.Rigidbody.velocity.y <= 0.01f && mController.IsGrounded)
            {
                mFSM.ChangeState(PlayerStateEnum.Idle);
            }
            
            if (Input.GetKeyDown(config.AttackKey))
            {
                mFSM.ChangeState(PlayerStateEnum.Attack);
            }
        }
    }
}
