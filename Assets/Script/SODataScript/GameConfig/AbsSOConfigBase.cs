using System;
using System.Collections.Generic;
using Alchemy.Serialization;
using UnityEngine;

namespace Script.SODataScript.GameConfig
{
    [AlchemySerialize]
    public abstract partial class AbsSOConfigBase<T>:ScriptableObject
    {
        [AlchemySerializeField,NonSerialized]
        protected Dictionary<int,T> _tbData = new();
        public abstract T Get(int id);
    }
}