using UnityEngine;
using System;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewItemShopConfig", menuName = "ConfigUtility/ItemShopConfig")]
    public class SOItemShopConfig:AbsDicScriptableObjectBase<ItemShopData>
    {
        public override ItemShopData Get(int id)
        {
            _ls = _data[id];
            return new ItemShopData()
            {
                Id = id,
                Name = _ls.Name,
            };
        }
    }
    
    [Serializable]
    public class ItemShopData
    {
        public int Id;//物品ID
        public string Name = "";
        public float Price;//物品价格
    }
}