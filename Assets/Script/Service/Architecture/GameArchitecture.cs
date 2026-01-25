using QFramework;
using Script.Service.Model;
using Script.Service.System;
using Script.Service.Utility;
using UnityEngine;

namespace Script.Service.Architecture
{
    public class GameArchitecture : Architecture<GameArchitecture>
    {
        private void RegisterModel()
        {
            
        }

        private void RegisterUtility()
        {
            RegisterUtility(new GameConfigUility());
        }

        private void RegisterSystem()
        {
            RegisterSystem(new GameManagerSystem());//游戏管理系统
            RegisterSystem(new SceneSwitchSystem());//游戏场景切换系统
            RegisterSystem(new MessageTipSystem());//消息弹窗系统
            RegisterSystem(new MouseCursorSystem());//鼠标光标系统
            RegisterSystem(new EnemySpawnSystem());//敌人生成系统
        }

        protected override void Init()
        {
#if !UNITY_EDITOR
            Debug.logger.logEnabled = false;
#endif
            Debug.Log("[GameArchitecture] Model开始注册...");
            RegisterModel();
            Debug.Log("[GameArchitecture] Utility开始注册...");
            RegisterUtility();
            Debug.Log("[GameArchitecture] System开始注册...");
            RegisterSystem();
        }
    }
}