using UnityEngine;

namespace Script.SODataScript.GameConfig
{
    [CreateAssetMenu(fileName = "NewGameConfig", menuName = "GameConfig/MainGameConfig")]
    public class SOGameConfig : ScriptableObject
    {
        [SerializeField]
        private SOEnemyConfig _enemyConfig;
        [SerializeField]
        private SOPlayerConfig _playerConfig;
        public SOEnemyConfig EnemyConfig => _enemyConfig;
        public SOPlayerConfig PlayerConfig => _playerConfig;
    }
}
