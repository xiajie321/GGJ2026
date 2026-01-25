using UnityEngine;

namespace Script.SODataScript.GameConfig
{
    [CreateAssetMenu(fileName = "NewGameConfig", menuName = "GameConfig/MainGameConfig")]
    public class SOGameConfig : ScriptableObject
    {
        public SOEnemyConfig EnemyConfig;
    }
}
