using System;
using System.Collections.Generic;
using Alchemy.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [AlchemySerialize]
    [ShowAlchemySerializationData]
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "ConfigUtility/LevelConfig")]
    public partial class SOLevelConfig:AbsDicScriptableObjectBase<LevelData>
    {
        public override LevelData Get(int id)
        {
            _ls = _data[id];
            return new LevelData()
            {
                Id = id,
                Name = _ls.Name,
                InitialMoney = _ls.InitialMoney,
                TrapCount = _ls.TrapCount,
                QualifiedLine = _ls.QualifiedLine,
                PerStarCost = _ls.PerStarCost,
                MaxStar = _ls.MaxStar,
                Ratio = _ls.Ratio,
            };
        }
    }
    
    [Serializable]
    public class LevelData
    {
        public int Id;
        public string Name = "";
        public float InitialMoney;//场景开始时为玩家提供初始金钱
        public int TrapCount;//场景在经过随机选择陷阱后的的陷阱总数

        public float QualifiedLine;//该场景的积分的及格线
        public float PerStarCost;//每颗星所需的积分
        public int MaxStar;//该场景的最高星级
        public float Ratio;//将积分转为货币的汇率，10积分=1货币则为0.1
    }
}