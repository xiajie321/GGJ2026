using System.Collections.Generic;
using Alchemy.Serialization;
using Alchemy.Inspector;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [AlchemySerialize]
    [ShowAlchemySerializationData]
    public abstract partial class AbsDicScriptableObjectBase<T>:ScriptableObject//需要字典的SO的数据基类
    {
        [AlchemySerializeField, System.NonSerialized]
        public Dictionary<int, T> _data = new();

        protected T _ls;
        public abstract T Get(int id);

#if UNITY_EDITOR
        [Alchemy.Inspector.Button]
        public void SaveConfig()
        {
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log($"{name} saved.");
        }
#endif
    }
}