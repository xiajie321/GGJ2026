using UnityEngine;

namespace Script.SODataScript.GameConfig
{
    [CreateAssetMenu(fileName = "NewGameConfig", menuName = "GameConfig/MainGameConfig")]
    public class SOGameConfig : ScriptableObject
    {
        [SerializeField]
        private readonly SOEnemyConfig _enemyConfig;
        public SOEnemyConfig EnemyConfig => _enemyConfig;
    }
}
