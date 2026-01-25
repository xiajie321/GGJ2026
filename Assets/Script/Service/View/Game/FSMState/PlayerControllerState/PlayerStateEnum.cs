namespace Script.Service.View.Game.FSMState.PlayerControllerState
{
    public enum PlayerStateEnum
    {
        Idle,       // 待机
        Move,       // 移动
        Jump,       // 跳跃
        Attack,     // 攻击
        Invincible  // 无敌（受伤后）
    }
}
