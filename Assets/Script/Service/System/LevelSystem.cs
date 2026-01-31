using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using QFramework;
using Script.Service.Model;
using Script.Service.Event;
using UnityEngine;
using System;

namespace Script.Service.System
{
    public class LevelSystem : AbstractSystem
    {
        private LevelModel _levelModel;
        private CancellationTokenSource _preparationCts;  // 准备阶段倒计时取消令牌

        protected override void OnInit()
        {
            _levelModel = this.GetModel<LevelModel>();
        }

        #region 关卡生命周期

        /// <summary>
        /// 开始关卡（初始化关卡数据）
        /// </summary>
        /// <param name="levelID">关卡ID</param>
        public void StartLevel(int levelID)
        {
            Debug.Log($"[cjh test] LevelSystem.StartLevel() 开始执行 - LevelID: {levelID}");
            
            // 取消之前的倒计时
            CancelPreparationTimer();
            Debug.Log($"[cjh test] LevelSystem.StartLevel() - 已取消之前的倒计时");
            
            // 选择关卡
            _levelModel.SelectLevel(levelID);
            Debug.Log($"[cjh test] LevelSystem.StartLevel() - 已调用 SelectLevel({levelID})");
            
            // 开始关卡（加载配置）
            _levelModel.StartLevel();
            Debug.Log($"[cjh test] LevelSystem.StartLevel() - 已调用 Model.StartLevel()");
            
            // 初始化金钱
            _levelModel.InitMoney();
            Debug.Log($"[cjh test] LevelSystem.StartLevel() - 已调用 InitMoney()，当前金钱: {_levelModel.Money}");
            
            // 初始化商店
            this.GetSystem<ShopSystem>().InitShop();
            Debug.Log($"[cjh test] LevelSystem.StartLevel() - 已调用 ShopSystem.InitShop()");
            
            // 进入第一波准备阶段（无倒计时）
            _levelModel.EnterPreparationState(isFirst: true);
            Debug.Log($"[cjh test] LevelSystem.StartLevel() - 已调用 EnterPreparationState(true)");
            
            // 发送关卡开始事件
            this.SendEvent<GameEnterEvent>();
            Debug.Log($"[cjh test] LevelSystem.StartLevel() - 已发送 GameEnterEvent");
            
            Debug.Log($"[cjh test] LevelSystem.StartLevel() 执行完成 - 关卡 {levelID} 开始，第一波准备阶段（无限时间）");
        }

        /// <summary>
        /// 重新开始当前关卡
        /// </summary>
        public void RestartLevel()
        {
            int currentLevelID = _levelModel.LevelID;
            _levelModel.ResetLevel();
            StartLevel(currentLevelID);
            
            // 发送重置事件
            this.SendEvent<GameResumeEvent>();
            
            Debug.Log($"[LevelSystem] 重新开始关卡 {currentLevelID}");
        }

        /// <summary>
        /// 结束关卡
        /// </summary>
        /// <param name="isSuccess">是否成功</param>
        public void EndLevel(bool isSuccess)
        {
            // 取消倒计时
            CancelPreparationTimer();
            
            _levelModel.EnterSettlementState(isSuccess);
            
            // 发送游戏退出事件
            this.SendEvent<GameExitEvent>();
            
            Debug.Log($"[LevelSystem] 关卡结束，结果：{(isSuccess ? "成功" : "失败")}");
        }

        #endregion

        #region 波次管理

        /// <summary>
        /// 开始新的波次（玩家手动或倒计时结束触发）
        /// </summary>
        public void StartWave()
        {
            // 取消准备阶段倒计时
            CancelPreparationTimer();
            
            // 检查是否已经在战斗中
            if (_levelModel.CurrentState == LevelState.Playing)
            {
                Debug.LogWarning("[LevelSystem] 已经在战斗中，无法重复开始");
                return;
            }

            // 进入战斗状态
            _levelModel.EnterPlayingState();

            Debug.Log($"[LevelSystem] 开始第 {_levelModel.CurrentWave} 波");

            // TODO: 通知怪物生成系统开始生成怪物
            // this.GetSystem<EnemySpawnSystem>().StartSpawning(_levelModel.CurrentWave);

            // 发送波次开始事件
            this.SendEvent(new OnWaveStartedEvent
            {
                WaveNumber = _levelModel.CurrentWave,
                TotalWaves = _levelModel.TotalWaves
            });
        }

        /// <summary>
        /// 波次完成（所有怪物被消灭）
        /// </summary>
        public void OnWaveCompleted()
        {
            Debug.Log($"[LevelSystem] 第 {_levelModel.CurrentWave} 波完成");

            // 发送波次完成事件
            this.SendEvent(new OnWaveCompletedEvent
            {
                WaveNumber = _levelModel.CurrentWave,
                TotalWaves = _levelModel.TotalWaves
            });

            // 检查是否是最后一波
            if (_levelModel.IsLastWave())
            {
                // 关卡成功
                EndLevel(isSuccess: true);
            }
            else
            {
                // 进入下一波准备阶段
                _levelModel.EnterPreparationState(isFirst: false);

                Debug.Log($"[LevelSystem] 进入准备阶段，倒计时 {_levelModel.PreparationTimeRemaining} 秒");

                // 发送准备阶段开始事件
                this.SendEvent(new OnPreparationStartedEvent
                {
                    NextWave = _levelModel.CurrentWave + 1,
                    PreparationTime = _levelModel.PreparationTimeRemaining
                });

                // 启动准备阶段倒计时
                StartPreparationTimer().Forget();
            }
        }

        /// <summary>
        /// 波次失败（玩家失败条件触发）
        /// </summary>
        public void OnWaveFailed()
        {
            Debug.Log($"[LevelSystem] 第 {_levelModel.CurrentWave} 波失败");

            // 发送波次失败事件
            this.SendEvent(new OnWaveFailedEvent
            {
                WaveNumber = _levelModel.CurrentWave
            });

            // 关卡失败
            EndLevel(isSuccess: false);
        }

        /// <summary>
        /// 玩家手动跳过准备时间
        /// </summary>
        public void SkipPreparationTime()
        {
            if (!_levelModel.IsInPreparation())
            {
                Debug.LogWarning("[LevelSystem] 不在准备阶段，无法跳过");
                return;
            }

            Debug.Log("[LevelSystem] 玩家跳过准备时间");
            StartWave();
        }

        #endregion

        #region 准备阶段倒计时管理

        /// <summary>
        /// 启动准备阶段倒计时（使用 UniTask）
        /// </summary>
        private async UniTaskVoid StartPreparationTimer()
        {
            // 取消之前的倒计时
            CancelPreparationTimer();
            
            // 创建新的取消令牌
            _preparationCts = new CancellationTokenSource();
            
            try
            {
                float remainingTime = _levelModel.PreparationTimeRemaining;
                
                // 倒计时循环
                while (remainingTime > 0)
                {
                    await UniTask.Delay(100, cancellationToken: _preparationCts.Token);  // 每100ms更新一次
                    
                    remainingTime -= 0.1f;
                    
                    // 更新 Model 中的时间
                    bool isFinished = _levelModel.UpdatePreparationTimer(0.1f);
                    
                    if (isFinished)
                    {
                        // 倒计时结束，自动开始波次
                        Debug.Log("[LevelSystem] 准备时间结束，自动开始波次");
                        StartWave();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LevelSystem] 准备阶段倒计时异常: {ex.Message}");
            }
            finally
            {
                _preparationCts?.Dispose();
                _preparationCts = null;
            }
        }

        /// <summary>
        /// 取消准备阶段倒计时
        /// </summary>
        private void CancelPreparationTimer()
        {
            if (_preparationCts != null)
            {
                _preparationCts.Cancel();
                _preparationCts.Dispose();
                _preparationCts = null;
            }
        }

        #endregion

        #region 获取关卡信息

        /// <summary>
        /// 获取当前关卡ID
        /// </summary>
        public int GetCurrentLevelID()
        {
            return _levelModel.LevelID;
        }

        /// <summary>
        /// 获取当前关卡状态
        /// </summary>
        public LevelState GetCurrentState()
        {
            return _levelModel.CurrentState;
        }

        /// <summary>
        /// 获取当前波次
        /// </summary>
        public int GetCurrentWave()
        {
            return _levelModel.CurrentWave;
        }

        /// <summary>
        /// 获取总波次数
        /// </summary>
        public int GetTotalWaves()
        {
            return _levelModel.TotalWaves;
        }

        /// <summary>
        /// 获取剩余准备时间
        /// </summary>
        public float GetPreparationTimeRemaining()
        {
            return _levelModel.PreparationTimeRemaining;
        }

        /// <summary>
        /// 获取关卡进度（0-1）
        /// </summary>
        public float GetLevelProgress()
        {
            return _levelModel.GetLevelProgress();
        }

        /// <summary>
        /// 是否在准备阶段（可以购买物品）
        /// </summary>
        public bool IsInPreparation()
        {
            return _levelModel.IsInPreparation();
        }

        #endregion
    }


}