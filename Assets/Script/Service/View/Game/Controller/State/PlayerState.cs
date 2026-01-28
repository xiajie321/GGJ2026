namespace Script.Service.View.Game.Controller
{
    public enum PlayerState//TODO 如有需要可以在这里补充状态
    {
        /// <summary>
        /// 静止状态
        /// </summary>
        Idle,
        /// <summary>
        /// 移动状态
        /// </summary>
        Move,
        /// <summary>
        /// 跳跃状态
        /// </summary>
        Jump,
        /// <summary>
        /// 下落状态
        /// </summary>
        Whereabouts,
        /// <summary>
        /// 攻击状态
        /// </summary>
        Attack,
        /// <summary>
        /// 无敌状态
        /// </summary>
        Invincible,
        /// <summary>
        /// 死亡状态
        /// </summary>
        Dead,
        /// <summary>
        /// 攻击状态
        /// </summary>
        Injured,
    }
}