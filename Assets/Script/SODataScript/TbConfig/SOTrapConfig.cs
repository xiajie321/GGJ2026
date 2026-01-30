using System;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewTrapConfig", menuName = "ConfigUtility/TrapConfig")]
    public class SOTrapConfig:AbsDicScriptableObjectBase<TrapData>
    {
        public override TrapData Get(int id)
        {
            _ls = _data[id];
            return new TrapData()
            {
                Id = id,
                Name = _ls.Name,
            };
        }
    }

    [Serializable]
    public class TrapData
    {
        public int Id;//这里可以不用填,因为在Get方法中会返回
        public string Name = "";
        public Sprite Sprite;
    }
}