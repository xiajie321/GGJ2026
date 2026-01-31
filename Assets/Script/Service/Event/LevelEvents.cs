using Scene = UnityEngine.SceneManagement.Scene;

namespace Script.Service.Event
{
    #region 关卡相关事件

    /// <summary>
    /// 波次开始事件
    /// </summary>
    public struct OnWaveStartedEvent
    {
        public int WaveNumber;
        public int TotalWaves;
    }

    /// <summary>
    /// 波次完成事件
    /// </summary>
    public struct OnWaveCompletedEvent
    {
        public int WaveNumber;
        public int TotalWaves;
    }

    /// <summary>
    /// 波次失败事件
    /// </summary>
    public struct OnWaveFailedEvent
    {
        public int WaveNumber;
    }

    /// <summary>
    /// 准备阶段开始事件
    /// </summary>
    public struct OnPreparationStartedEvent
    {
        public int NextWave;
        public float PreparationTime;
    }

    /// <summary>
    /// 积分/金钱变化事件
    /// </summary>
    public struct OnMoneyChangedEvent
    {
        public float NewMoney;
    }

    #endregion
}