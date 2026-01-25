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
            // TODO mController.Animator.Play("Attack");
            
            // 开启攻击判定
            if (mController.AttackGameObject != null)
            {
                mController.AttackGameObject.SetActive(true);
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
