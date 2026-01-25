using QFramework;
using Script.Service.View.Game.Controller;
using UnityEngine;
using System.Collections.Generic;

namespace Script.Service.View.Game.FSMState.PlayerControllerState
{
    public class AttackState : AbstractPlayerState
    {
        private float mTimer;
        private float mAttackDuration = 0.5f; // 假设攻击动画时长0.5s

        public AttackState(FSM<PlayerStateEnum> fsm, PlayerController target) : base(fsm, target)
        {
        }

        protected override void OnEnter()
        {
            mTimer = 0;
            // 播放攻击动画
            // mController.Animator.Play("Attack");
            
            // 开启攻击判定
            if (mController.AttackGameObject != null)
            {
                mController.AttackGameObject.SetActive(true);
            }
            
            // 执行攻击逻辑：扇形攻击
            // 需求：前方小范围（跳跃高度能通过配置表拿到）扇形攻击，击杀所有攻击框内的僵尸
            // 注意：虽然有AttackGameObject，但需求描述了扇形攻击的具体逻辑，这里实现扇形检测
            PerformSectorAttack();
        }

        private void PerformSectorAttack()
        {
            float attackRange = mController.GameConfig.GameConfig.PlayerConfig.JumpHeight; // 使用跳跃高度作为攻击范围
            float attackAngle = 60f; // 假设扇形角度60度
            
            // 获取所有在攻击范围内的碰撞体
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mController.transform.position, attackRange);
            
            // 获取玩家朝向
            Vector3 playerDirection = mController.transform.localScale.x > 0 ? Vector3.right : Vector3.left;
            
            foreach (var hitCollider in hitColliders)
            {
                // 尝试获取 EnemyController
                EnemyController enemy = hitCollider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    Vector3 directionToEnemy = (enemy.transform.position - mController.transform.position).normalized;
                    float angle = Vector3.Angle(playerDirection, directionToEnemy);
                    
                    // 判断是否在扇形角度内
                    if (angle < attackAngle / 2)
                    {
                        // 击杀僵尸
                        Object.Destroy(enemy.gameObject);
                        // 或者调用 enemy.Die() 如果有这个方法
                    }
                }
            }
        }

        protected override void OnUpdate()
        {
            mTimer += Time.deltaTime;
            if (mTimer >= mAttackDuration)
            {
                mFSM.ChangeState(PlayerStateEnum.Idle);
            }
        }

        protected override void OnExit()
        {
            // 关闭攻击判定
            if (mController.AttackGameObject != null)
            {
                mController.AttackGameObject.SetActive(false);
            }
        }
    }
}
