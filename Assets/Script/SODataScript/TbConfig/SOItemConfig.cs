using System;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewItemConfig", menuName = "ConfigUtility/ItemConfig")]
    public class SOItemConfig:AbsDicScriptableObjectBase<ItemData>
    {
        public override ItemData Get(int id)
        {
            _ls = _data[id];
            return new ItemData()
            {
                Id = id,
                Name = _ls.Name,
                Sprite = _ls.Sprite,
            };
        }
    }
    [Serializable]
    public class ItemData
    {
        public int Id;//这里可以不用填,因为在Get方法中会返回
        public string Name = "";
        public Sprite Sprite;
    }
}