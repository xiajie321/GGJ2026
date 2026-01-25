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

        public TbEnemyConfig GetEnemyConfig(int enemyId = 1)
        {
            if (GameConfig != null && GameConfig.EnemyConfig != null)
            {
                return GameConfig.EnemyConfig.Get(enemyId);
            }

            Debug.LogWarning("[GameConfigUility] 使用默认敌人配置");
            return new TbEnemyConfig
            {
                Name = "DefaultEnemy",
                Hp = 100,
                Speed = 3f
            };
        }
    }
}