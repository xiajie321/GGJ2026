using QFramework;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game.FSMState.PlayerControllerState
{
    public abstract class AbstractPlayerState : AbstractState<PlayerStateEnum, PlayerController>
    {
        protected PlayerController mController;

        public AbstractPlayerState(FSM<PlayerStateEnum> fsm, PlayerController target) : base(fsm, target)
        {
            mController = target;
        }
        
        // 辅助方法：面向鼠标
        protected void FaceMouse()
        {
            if (Camera.main == null) return;
            
            // 获取鼠标在世界坐标的位置
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 角色朝向鼠标
            // 需求说明：左右移动不影响角色朝向，角色始终面向玩家鼠标
            
            // 简单的翻转实现
            Vector3 scale = mController.transform.localScale;
            if (mousePos.x < mController.transform.position.x)
            {
                scale.x = -Mathf.Abs(scale.x);
            }
            else
            {
                scale.x = Mathf.Abs(scale.x);
            }
            mController.transform.localScale = scale;
        }
    }
}
