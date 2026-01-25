using QFramework;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game.FSMState.PlayerControllerState
{
    public class MoveState : AbstractPlayerState
    {
        public MoveState(FSM<PlayerStateEnum> fsm, PlayerController target) : base(fsm, target)
        {
        }

        protected override void OnEnter()
        {
            // 播放移动动画
            // TODO mController.Animator.Play("Move");
        }

        protected override void OnUpdate()
        {
            FaceMouse();

            var config = mController.GameConfig.GameConfig.PlayerConfig;
            float moveX = 0;

            if (Input.GetKey(config.MoveLeftKey))
            {
                // 如果左边没有碰到墙，允许向左移动
                if (!mController.IsTouchingLeftWall)
                {
                    moveX = -1;
                }
            }
            else if (Input.GetKey(config.MoveRightKey))
            {
                // 如果右边没有碰到墙，允许向右移动
                if (!mController.IsTouchingRightWall)
                {
                    moveX = 1;
                }
            }

            if (moveX == 0)
            {
                // 如果本来想移动但因为撞墙而导致moveX为0，仍然可能需要切换到Idle
                // 但如果按键都没有按下，那肯定是切换到Idle
                if (!Input.GetKey(config.MoveLeftKey) && !Input.GetKey(config.MoveRightKey))
                {
                    mFSM.ChangeState(PlayerStateEnum.Idle);
                    return;
                }
                
                // 如果按键按下了但是撞墙了，也停下来
                if (mController.Rigidbody != null)
                {
                    Vector2 velocity = mController.Rigidbody.velocity;
                    velocity.x = 0;
                    mController.Rigidbody.velocity = velocity;
                }
            }
            else
            {
                // 移动逻辑
                if (mController.Rigidbody != null)
                {
                    Vector2 velocity = mController.Rigidbody.velocity;
                    velocity.x = moveX * mController.Speed;
                    mController.Rigidbody.velocity = velocity;
                }
            }

            if (Input.GetKey(config.JumpKey) && mController.IsGrounded)
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
