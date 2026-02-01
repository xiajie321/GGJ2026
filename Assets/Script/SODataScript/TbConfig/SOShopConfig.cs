using System;
using System.Collections.Generic;
using Alchemy.Serialization;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [AlchemySerialize]
    [ShowAlchemySerializationData]
    [CreateAssetMenu(fileName = "NewShopConfig", menuName = "ConfigUtility/ShopConfig")]
    public partial class SOShopConfig:AbsDicScriptableObjectBase<ShopConfig>
    {
        public override ShopConfig Get(int id)
        {
            _ls = _data[id];
            return new ShopConfig()
            {
                Id = id,
                Name = _ls.Name,
                BuySound = _ls.BuySound,
                Shops = _ls.GetShops(),
            };
        }
    }
    [Serializable]
    public class ShopConfig
    {
        public int Id;
        public string Name = "";
        public List<ShopConfigData> Shops = new();
        
        public AudioClip BuySound;//交易成功时的音效

        
        public List<ShopConfigData> GetShops()//深拷贝数组
        {
            List<ShopConfigData> _ls = new();
            for (int i = 0; i < Shops.Count; i++)
            {
                _ls.Add(new ShopConfigData()
                {
                    Id = Shops[i].Id,
                });
            }
            return _ls;
        }
    }

    [Serializable]
    public class ShopConfigData
    {
        public int Id;

        
    }
}