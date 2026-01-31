using System;
using System.Collections.Generic;
using Alchemy.Serialization;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [AlchemySerialize]
    [ShowAlchemySerializationData]
    [CreateAssetMenu(fileName = "NewTrapConfig", menuName = "ConfigUtility/TrapConfig")]
    public partial class SOTrapConfig:AbsDicScriptableObjectBase<TrapData>
    {
        public override TrapData Get(int id)
        {
            _ls = _data[id];
            return new TrapData()
            {
                Id = id,
                Name = _ls.Name,
                Sprite = _ls.Sprite,
                Height = _ls.Height,
                Level = _ls.Level,
            };
        }
    }

    [Serializable]
    public class TrapData
    {
        public int Id;//这里可以不用填,因为在Get方法中会返回
        public string Name = "";
        public Sprite Sprite;
        
        public Height Height;//陷阱高度
        public TrapLevel Level;//陷阱等级
    }

    [Serializable]
    public enum TrapLevel
    {
        S = 1,
        A = 3,
        B = 5,
    }
}