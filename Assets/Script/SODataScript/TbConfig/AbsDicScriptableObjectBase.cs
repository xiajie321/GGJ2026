using System.Collections.Generic;
using Alchemy.Serialization;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [AlchemySerialize]
    public abstract partial class AbsDicScriptableObjectBase<T>:ScriptableObject//需要字典的SO的数据基类
    {
        [AlchemySerializeField]
        protected Dictionary<int,T> _data = new();
        public abstract T Get(int id);
    }
}