using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using Script.Service.Utility;
using Script.SODataScript.TbConfig;

namespace Script.Service.Model
{

    public class LevelModel : AbstractModel
    {
        private int _levelID = 0;
        public int LevelID=>_levelID;
        protected override void OnInit()
        {
            //默认关卡为0
            ResetLevel();
        }

        //选关
        public void SelectLevel(int levelID)
        {
            _levelID = levelID;
        }

        //重置关卡
        public void ResetLevel()
        {
            _levelID = 0;
        }


        //获取关卡数据
        public LevelData GetLevelData()
        {
            // 修正：假设 TbLevelConfig 是 ConfigUtility 的一个字段或属性而不是 Config 的
           return this.GetUtility<ConfigUtility>().Config.TbLevelConfig.Get(_levelID);
        }

    }
}
