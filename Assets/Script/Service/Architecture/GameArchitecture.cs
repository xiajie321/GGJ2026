using QFramework;
using Script.Service.Event;
using Script.Service.Model;
using Script.Service.System;
using Script.Service.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Service.Architecture
{
    public class GameArchitecture : Architecture<GameArchitecture>
    {
        private void RegisterModel()
        {
            RegisterModel(new GameModel());
        }

        private void RegisterUtility()
        {
            RegisterUtility(new ConfigUtility());
        }

        private void RegisterSystem()
        {
            RegisterSystem(new GameManagerSystem());
            RegisterSystem(new DamageFloatingTextSystem());
            RegisterSystem(new SceneSwitchSystem());
            RegisterSystem(new MessageTipSystem());
            RegisterSystem(new MouseCursorSystem());
            RegisterSystem(new FactorySystem());
            RegisterSystem(new CameraEdgeScrollingSystem());
        }

        protected override void Init()
        {
#if !UNITY_EDITOR
            Debug.unityLogger.logEnabled = false;
#endif
            SceneManager.activeSceneChanged += (a, b) =>
            {
                SendEvent(new SceneChangeEvent()
                {
                    CurrentScene = a,
                    TargetScene = b
                });
            };
            Debug.Log("[GameArchitecture] Model开始注册...");
            RegisterModel();
            Debug.Log("[GameArchitecture] Utility开始注册...");
            RegisterUtility();
            Debug.Log("[GameArchitecture] System开始注册...");
            RegisterSystem();
        }
    }
}