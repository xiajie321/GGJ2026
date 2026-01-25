using QFramework;
using Script.SODataScript.GameConfig;
using UnityEngine;

namespace Script.Service.Utility
{
    public class GameConfigUility:IUtility
    {
        private SOGameConfig _gameConfig;
        public SOGameConfig GameConfig
        {
            get
            {
                if(!_gameConfig) Init();
                return _gameConfig;
            }
        }
        void Init()
        {
            _gameConfig = Resources.Load<SOGameConfig>("SOData/GameConfig/MainGameConfig");
            Debug.Log("[GameConfigUility] 加载完成...");
        }
    }
}