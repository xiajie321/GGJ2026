using System.Collections;
using System.Collections.Generic;
using QFramework;
using Script.Service.Utility;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.Model
{
    /// <summary>
    /// 关卡状态枚举
    /// </summary>
    public enum LevelState
    {
        /// <summary>
        /// 第一波刷怪前的准备阶段 - 无限的准备时间
        /// </summary>
        PreparationFirst = 1,

        /// <summary>
        /// 战斗阶段 - 怪物正在进攻
        /// </summary>
        Playing = 2,

        /// <summary>
        /// 中间的准备阶段 - 波次间隙，有倒计时
        /// </summary>
        PreparationBetween = 3,

        /// <summary>
        /// 结算阶段 - 关卡结束（成功或失败）
        /// </summary>
        Settlement = 4,
    }

    public class LevelModel : AbstractModel
    {
        #region 字段和属性

        private int _levelID = 0;
        private LevelState _levelState = LevelState.PreparationFirst;
        private float _preparationTimeRemaining = 0f;  // 剩余准备时间（秒）
        private int _currentWave = 0;  // 当前波次
        private int _totalWaves = 0;   // 总波次数

        // 配置：每个波次间的准备时间
        private const float PREPARATION_DURATION = 15f;

        /// <summary>
        /// 当前关卡ID
        /// </summary>
        public int LevelID => _levelID;

        /// <summary>
        /// 当前关卡状态
        /// </summary>
        public LevelState CurrentState => _levelState;

        /// <summary>
        /// 剩余准备时间（秒），-1表示无限时间
        /// </summary>
        public float PreparationTimeRemaining => _preparationTimeRemaining;

        /// <summary>
        /// 当前波次（从1开始）
        /// </summary>
        public int CurrentWave => _currentWave;

        /// <summary>
        /// 总波次数
        /// </summary>
        public int TotalWaves => _totalWaves;

        #endregion

        #region 初始化

        protected override void OnInit()
        {
            ResetLevel();
            _levelState = LevelState.PreparationFirst;
        }

        /// <summary>
        /// 选择关卡
        /// </summary>
        /// <param name="levelID">关卡ID</param>
        public void SelectLevel(int levelID)
        {
            _levelID = levelID;
        }

        /// <summary>
        /// 重置关卡数据
        /// </summary>
        public void ResetLevel()
        {
            _levelID = 0;
            _levelState = LevelState.PreparationFirst;
            _preparationTimeRemaining = 0f;
            _currentWave = 0;
            _totalWaves = 0;
        }

        /// <summary>
        /// 开始关卡（加载关卡配置）
        /// </summary>
        public void StartLevel()
        {
            _currentWave = 0;
            _levelState = LevelState.PreparationFirst;
            _preparationTimeRemaining = -1f;  // 第一波无限时间

            // 从配置加载总波次
            var levelData = GetLevelData();
            _totalWaves = levelData.TrapCount;  // 假设使用 TrapCount 作为波次数，可根据实际调整
        }

        #endregion

        #region 状态管理

        /// <summary>
        /// 进入准备阶段
        /// </summary>
        /// <param name="isFirst">是否是第一波前的准备</param>
        public void EnterPreparationState(bool isFirst)
        {
            _levelState = isFirst ? LevelState.PreparationFirst : LevelState.PreparationBetween;

            // 第一波无限时间（-1），中间波次有倒计时
            _preparationTimeRemaining = isFirst ? -1f : PREPARATION_DURATION;
        }

        /// <summary>
        /// 进入战斗阶段
        /// </summary>
        public void EnterPlayingState()
        {
            _levelState = LevelState.Playing;
            _preparationTimeRemaining = 0f;
            _currentWave++;
        }

        /// <summary>
        /// 进入结算阶段
        /// </summary>
        /// <param name="isSuccess">是否成功通关</param>
        public void EnterSettlementState(bool isSuccess = true)
        {
            _levelState = LevelState.Settlement;
            _preparationTimeRemaining = 0f;
        }

        /// <summary>
        /// 检查是否在准备阶段
        /// </summary>
        /// <returns>是否在准备阶段</returns>
        public bool IsInPreparation()
        {
            return _levelState == LevelState.PreparationFirst ||
                   _levelState == LevelState.PreparationBetween;
        }

        /// <summary>
        /// 检查是否是最后一波
        /// </summary>
        /// <returns>是否是最后一波</returns>
        public bool IsLastWave()
        {
            return _currentWave >= _totalWaves;
        }

        #endregion

        #region 倒计时相关

        /// <summary>
        /// 更新准备时间倒计时（由 System 在 Update 中调用）
        /// </summary>
        /// <param name="deltaTime">时间增量</param>
        /// <returns>是否倒计时结束</returns>
        public bool UpdatePreparationTimer(float deltaTime)
        {
            // 第一波（无限时间）或非准备阶段，不倒计时
            if (_levelState != LevelState.PreparationBetween || _preparationTimeRemaining < 0)
            {
                return false;
            }

            _preparationTimeRemaining -= deltaTime;

            // 倒计时结束
            if (_preparationTimeRemaining <= 0f)
            {
                _preparationTimeRemaining = 0f;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 手动开始战斗（跳过准备时间）
        /// </summary>
        public void SkipPreparation()
        {
            if (IsInPreparation())
            {
                EnterPlayingState();
            }
        }

        /// <summary>
        /// 获取倒计时显示文本（格式化为 MM:SS）
        /// </summary>
        /// <returns>倒计时文本</returns>
        public string GetTimerText()
        {
            if (_preparationTimeRemaining < 0)
            {
                return "∞";  // 无限时间
            }

            int minutes = Mathf.FloorToInt(_preparationTimeRemaining / 60f);
            int seconds = Mathf.FloorToInt(_preparationTimeRemaining % 60f);
            return $"{minutes:00}:{seconds:00}";
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取当前关卡配置数据
        /// </summary>
        /// <returns>关卡数据</returns>
        public LevelData GetLevelData()
        {
            return this.GetUtility<ConfigUtility>().Config.TbLevelConfig.Get(_levelID);
        }

        /// <summary>
        /// 获取关卡进度百分比（用于UI显示）
        /// </summary>
        /// <returns>进度百分比 (0-1)</returns>
        public float GetLevelProgress()
        {
            if (_totalWaves <= 0) return 0f;
            return (float)_currentWave / _totalWaves;
        }

        #endregion
    }
}