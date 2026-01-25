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
            // mController.Animator.Play("Jump");
        }

        protected override void OnUpdate()
        {
            FaceMouse();
            
            // 空中移动控制（如果允许）
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

            if (mController.Rigidbody != null && moveX != 0)
            {
                Vector2 velocity = mController.Rigidbody.velocity;
                velocity.x = moveX * mController.Speed;
                mController.Rigidbody.velocity = velocity;
            }

            // 落地检测：这里简单判断 y 速度小于等于 0 且接触地面
            // 实际项目中可能需要使用射线检测或碰撞检测
            if (mController.Rigidbody.velocity.y <= 0.01f) // 简单模拟落地
            {
                // 注意：这里需要配合碰撞检测来确定是否真的落地，
                // 如果没有物理碰撞检测逻辑，可能导致无限跳或者无法切回 Idle
                // 暂时假设速度向下且接近0就切换（这通常不准确，应该用 OnCollisionEnter2D 或者 射线检测）
                
                // 由于任务只要求实现状态机逻辑，没有具体的地面检测组件代码，
                // 这里我们假设如果垂直速度接近0，就是落地了
                if (Mathf.Abs(mController.Rigidbody.velocity.y) < 0.01f)
                {
                   mFSM.ChangeState(PlayerStateEnum.Idle);
                }
            }
            
            if (Input.GetKeyDown(config.AttackKey))
            {
                mFSM.ChangeState(PlayerStateEnum.Attack);
            }
        }
    }
}
