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
            // mController.Animator.Play("Move");
        }

        protected override void OnUpdate()
        {
            FaceMouse();

            var config = mController.GameConfig.GameConfig.PlayerConfig;
            float moveX = 0;

            if (Input.GetKey(config.MoveLeftKey))
            {
                moveX = -1;
            }
            else if (Input.GetKey(config.MoveRightKey))
            {
                moveX = 1;
            }

            if (moveX == 0)
            {
                mFSM.ChangeState(PlayerStateEnum.Idle);
                return;
            }

            // 移动逻辑
            if (mController.Rigidbody != null)
            {
                Vector2 velocity = mController.Rigidbody.velocity;
                velocity.x = moveX * mController.Speed;
                mController.Rigidbody.velocity = velocity;
            }

            if (Input.GetKeyDown(config.JumpKey))
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
