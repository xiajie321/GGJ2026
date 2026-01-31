using Alchemy.Serialization;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Script.SODataScript.TbConfig
{
    [AlchemySerialize]
    [ShowAlchemySerializationData]
    [CreateAssetMenu(fileName = "NewItemShopConfig", menuName = "ConfigUtility/ItemShopConfig")]
    public partial class SOItemShopConfig:AbsDicScriptableObjectBase<ItemShopData>
    {
        public override ItemShopData Get(int id)
        {
            _ls = _data[id];
            return new ItemShopData()
            {
                Id = id,
                Name = _ls.Name,
                Price = _ls.Price
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