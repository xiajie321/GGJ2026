using QFramework;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game.FSMState.PlayerControllerState
{
    public class InvincibleState : AbstractPlayerState
    {
        private float mTimer;
        private float mInvincibleDuration;
        private float mFlashInterval = 0.1f; // 闪烁间隔
        private float mFlashTimer;
        private SpriteRenderer mSpriteRenderer;

        public InvincibleState(FSM<PlayerStateEnum> fsm, PlayerController target) : base(fsm, target)
        {
        }

        protected override void OnEnter()
        {
            mInvincibleDuration = mController.GameConfig.GameConfig.PlayerConfig.InvincibilityTime;
            mTimer = 0;
            mFlashTimer = 0;
            
            // 获取 SpriteRenderer 以便控制颜色/透明度来实现闪烁
            mSpriteRenderer = mController.GetComponent<SpriteRenderer>();
            
            // 变红表示受伤
            if (mSpriteRenderer != null)
            {
                mSpriteRenderer.color = Color.red;
            }
        }

        protected override void OnUpdate()
        {
            // 处理移动逻辑（无敌状态下通常也可以移动）
            // 复用 MoveState 的逻辑，或者是允许移动但不切换状态
            HandleMovement();
            
            mTimer += Time.deltaTime;
            mFlashTimer += Time.deltaTime;

            // 闪烁效果 (红 -> 正常/透明 -> 红)
            if (mFlashTimer >= mFlashInterval)
            {
                mFlashTimer = 0;
                if (mSpriteRenderer != null)
                {
                    // 简单的闪烁实现：切换透明度
                    Color color = mSpriteRenderer.color;
                    // 如果是红色，变回白色带透明；如果是白色带透明，变回红色
                    if (color == Color.red)
                    {
                         color = new Color(1, 1, 1, 0.5f);
                    }
                    else
                    {
                        color = Color.red;
                    }
                    mSpriteRenderer.color = color;
                }
            }

            if (mTimer >= mInvincibleDuration)
            {
                mFSM.ChangeState(PlayerStateEnum.Idle);
            }
        }

        protected override void OnExit()
        {
            // 恢复颜色
            if (mSpriteRenderer != null)
            {
                mSpriteRenderer.color = Color.white;
            }
        }
        
        // 简单的移动逻辑处理，允许在受伤无敌状态下移动
        private void HandleMovement()
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

            if (mController.Rigidbody != null)
            {
                Vector2 velocity = mController.Rigidbody.velocity;
                velocity.x = moveX * mController.Speed;
                mController.Rigidbody.velocity = velocity;
            }
            
            if (Input.GetKeyDown(config.JumpKey) && mController.Rigidbody.velocity.y <= 0.01f)
            {
                 // 允许无敌状态跳跃
                 // 简单的跳跃逻辑复用
                 if (mController.Rigidbody != null)
                 {
                    float jumpHeight = config.JumpHeight;
                    float gravity = Mathf.Abs(Physics2D.gravity.y * mController.Rigidbody.gravityScale);
                    float jumpSpeed = Mathf.Sqrt(2 * gravity * jumpHeight);

                    Vector2 velocity = mController.Rigidbody.velocity;
                    velocity.y = jumpSpeed;
                    mController.Rigidbody.velocity = velocity;
                 }
            }
        }
    }
}
