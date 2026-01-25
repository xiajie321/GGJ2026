using QFramework;
using Script.SODataScript.GameConfig;
using UnityEngine;
namespace Script.Service.Model
{
    public class GameConfigModel:AbstractModel
    {
        private SOGameConfig _gameConfig;
        public SOGameConfig GameConfig => _gameConfig;
        public GameObject PlayerObject;
        protected override void OnInit()
        {
            _gameConfig = Resources.Load<SOGameConfig>("SOData/GameConfig/MainGameConfig");
            Debug.Log("[GameConfigModel] 加载完成...");
        }
    }
}