using QFramework;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.Utility
{
    public class ConfigUtility:IUtility
    { 
        SOMainConfig _mainConfig;
        public SOMainConfig Config
        {
            get
            {
                if (!_mainConfig)
                {
                    _mainConfig = Resources.Load<SOMainConfig>("SOData/ConfigUitlity/MainConfig");
                }
                return _mainConfig;
            }
        }
    }
}