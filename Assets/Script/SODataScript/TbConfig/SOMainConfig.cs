using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewMainConfig", menuName = "ConfigUtility/MainConfig")]
    public class SOMainConfig:ScriptableObject
    {
        [SerializeField]
        private SOTestDataConfig _tbTestDataConfig;
        public SOTestDataConfig TbTestDataConfig => _tbTestDataConfig;
        
        [SerializeField] private SOEnemyConfig _tbEnemyConfig;
        public SOEnemyConfig TbEnemyConfig => _tbEnemyConfig;
        
        [SerializeField] private SOItemConfig _tbItemConfig;
        public SOItemConfig TbItemConfig => _tbItemConfig;
        [SerializeField] private SOTrapConfig _tbTrapConfig;
        public SOTrapConfig TbTrapConfig => _tbTrapConfig;
        [SerializeField] private SOEnemyGeneratorConfig _tbEnemyGeneratorConfig;
        public SOEnemyGeneratorConfig TbEnemyGeneratorConfig => _tbEnemyGeneratorConfig;
    }
}