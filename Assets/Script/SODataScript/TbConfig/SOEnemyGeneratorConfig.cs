using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewEnemyGeneratorConfig", menuName = "ConfigUtility/EnemyGeneratorConfig")]
    public class SOEnemyGeneratorConfig:AbsDicScriptableObjectBase<EnemyGeneratorConfig>
    {
        public override EnemyGeneratorConfig Get(int id)
        {
            _ls = _data[id];
            return new EnemyGeneratorConfig()
            {
                Id = id,
                Name = _ls.Name,
                UpdateTime = _ls.UpdateTime,
                Enemys = _ls.GetEnemys(),
            };
        }
    }
    [Serializable]
    public class EnemyGeneratorConfig
    {
        public int Id;
        public string Name = "";
        public float UpdateTime;//刷新时间
        public List<EnemyGeneratorConfigData> Enemys = new();
        public List<EnemyGeneratorConfigData> GetEnemys()//深拷贝数组
        {
            List<EnemyGeneratorConfigData> _ls = new();
            for (int i = 0; i < Enemys.Count; i++)
            {
                _ls.Add(new EnemyGeneratorConfigData(Enemys[i]));
            }
            return _ls;
        }
    }
    [Serializable]
    public class EnemyGeneratorConfigData
    {
        public EnemyGeneratorConfigData(EnemyGeneratorConfigData data)
        {
            Id = data.Id;
            Sum =  data.Sum;
        }
        public int Id;
        public int Sum;
    }
}