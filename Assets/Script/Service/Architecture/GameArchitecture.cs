using QFramework;
using Script.Service.System;
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
            
        }
        private void RegisterSystem()
        {
            RegisterSystem(new MessageTipSystem());
            RegisterSystem(new MouseCursorSystem());
        }
        protected override void Init()
        {
            Debug.Log("[GameArchitecture] Model开始注册...");
            RegisterModel();
            Debug.Log("[GameArchitecture] Model注册完毕...");
            Debug.Log("[GameArchitecture] Utility开始注册...");
            RegisterUtility();
            Debug.Log("[GameArchitecture] Utility注册完毕...");
            Debug.Log("[GameArchitecture] System开始注册...");
            RegisterSystem();
            Debug.Log("[GameArchitecture] System注册完毕...");
        }
    }
}
