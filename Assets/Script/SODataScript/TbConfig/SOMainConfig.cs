using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewMainConfig", menuName = "ConfigUtility/MainConfig")]
    public class SOMainConfig:ScriptableObject
    {
        [SerializeField]
        private SOTestDataConfig _tbTestDataConfig;
        public SOTestDataConfig TbTestDataConfig => _tbTestDataConfig;
    }
}