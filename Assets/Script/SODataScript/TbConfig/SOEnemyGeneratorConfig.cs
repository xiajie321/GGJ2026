using System;
using System.Collections.Generic;
using Alchemy.Serialization;
using NUnit.Framework;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [AlchemySerialize]
    [ShowAlchemySerializationData]
    [CreateAssetMenu(fileName = "NewEnemyGeneratorConfig", menuName = "ConfigUtility/EnemyGeneratorConfig")]
    public partial class SOEnemyGeneratorConfig:AbsDicScriptableObjectBase<EnemyGeneratorConfig>
    {
        public override EnemyGeneratorConfig Get(int id)
        {
            _ls = _data[id];
            return new EnemyGeneratorConfig()
            {
                Id = id,
                Name = _ls.Name,
                Enemys = _ls.GetEnemys(),
            };
        }
    }
    [Serializable]
    public class EnemyGeneratorConfig
    {
        public int Id;
        public string Name = "";
        public List<EnemyGeneratorConfigData> Enemys = new();
        public List<EnemyGeneratorConfigData> GetEnemys()//深拷贝数组
        {
            List<EnemyGeneratorConfigData> _ls = new();
            for (int i = 0; i < Enemys.Count; i++)
            {
                _ls.Add(new EnemyGeneratorConfigData().SetData(Enemys[i]));
            }
            return _ls;
        }
    }
    [Serializable]
    public class EnemyGeneratorConfigData
    {
        public EnemyGeneratorConfigData SetData(EnemyGeneratorConfigData data)
        {
            UpdateTime = data.UpdateTime;
            Data = data.GetData();
            return this;
        }
        public float UpdateTime;
        public List<EnemyGeneratorConfigDataItem> Data = new();
        public List<EnemyGeneratorConfigDataItem> GetData()//深拷贝数据
        {
            List<EnemyGeneratorConfigDataItem> _ls = new();
            for (int i = 0; i < Data.Count; i++)
            {
                _ls.Add(new EnemyGeneratorConfigDataItem()
                {
                    Id = Data[i].Id,
                    Sum = Data[i].Sum,
                });
            }
            return _ls;
        }
    }
    [Serializable]
    public class EnemyGeneratorConfigDataItem
    {
        public int Id;
        public int Sum;
    }
}