using System.Collections;
using System.Collections.Generic;
using QFramework;
using Script.Service.Utility;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.Model
{
    public class ShopModel : AbstractModel
    {
        #region 字段和属性

        //商店槽位和购买次数
        private List<int> _shopContainer = new List<int>();
        //商店槽位和商品ID
        private List<int> _shopContainerItemID = new List<int>();
        //商店槽位当前商品在商品列表中的索引
        private List<int> _shopContainerItemIndex = new List<int>();
        //商店允许出售的所有物品
        private List<ShopConfigData> _shopItems = new List<ShopConfigData>();

        private float _upPriceRadio = 0.2f;
        private int _slotCount = 6;

        #endregion

        #region 初始化

        protected override void OnInit()
        {

        }

        /// <summary>
        /// 初始化商店容器（游戏开始时调用）- 清零所有数据
        /// </summary>
        public void InitShopContainer()
        {
            Debug.Log($"[cjh test] ShopModel.InitShopContainer() 开始执行");
            
            _shopContainer.Clear();
            _shopContainerItemID.Clear();
            _shopContainerItemIndex.Clear();
            //暂定为6个槽位
            for (int i = 0; i < _slotCount; i++)
            {
                _shopContainer.Add(0);  // 购买次数初始化为0
                _shopContainerItemID.Add(-1);  // 商品ID初始化为-1（表示空）
                _shopContainerItemIndex.Add(i);  // 商品索引初始化为槽位索引
            }
            
            Debug.Log($"[cjh test] ShopModel.InitShopContainer() 完成 - 槽位数: {_slotCount}");
        }

        /// <summary>
        /// 重置商店所有槽位的商品（不清零购买次数）
        /// </summary>
        /// <param name="shopID">关卡ID（对应商店ID）</param>
        public void ResetShopContainer(int shopID)
        {
            Debug.Log($"[cjh test] ShopModel.ResetShopContainer() 开始执行 - ShopID: {shopID}");
            
            SetShopItems(shopID);
            Debug.Log($"[cjh test] ShopModel.ResetShopContainer() - 已加载商品列表");
            
            // 初始化：每个槽位根据自己的索引获取对应的商品
            for (int i = 0; i < _slotCount; i++)
            {
                SetShopItemBySlotIndex(i);
            }
            
            Debug.Log($"[cjh test] ShopModel.ResetShopContainer() 完成 - 已设置 {_slotCount} 个槽位");
        }

        #endregion

        #region 设置相关

        /// <summary>
        /// 加载关卡对应的商品列表
        /// </summary>
        /// <param name="shopID">关卡ID（对应商店ID）</param>
        public void SetShopItems(int shopID)
        {
            Debug.Log($"[cjh test] ShopModel.SetShopItems() - 尝试加载 ShopID: {shopID}");
            
            var shopConfig = this.GetUtility<ConfigUtility>().Config.TbShopConfig.Get(shopID);
            
            if (shopConfig == null)
            {
                Debug.LogError($"[cjh test] ShopModel.SetShopItems() - 错误：找不到商店配置！ShopID: {shopID}");
                _shopItems = new List<ShopConfigData>();
                return;
            }
            
            _shopItems = shopConfig.Shops;
            Debug.Log($"[cjh test] ShopModel.SetShopItems() 完成 - 商品数量: {_shopItems?.Count ?? 0}");
        }

        /// <summary>
        /// 根据槽位自己的商品索引设置商品（初始化时使用）
        /// </summary>
        /// <param name="containerID">槽位索引</param>
        private void SetShopItemBySlotIndex(int containerID)
        {
            if (_shopItems == null || _shopItems.Count == 0)
            {
                Debug.LogWarning($"[cjh test] ShopModel.SetShopItemBySlotIndex() - 警告：商品列表为空，无法设置槽位 {containerID}");
                return;
            }

            if (containerID < 0 || containerID >= _shopContainerItemID.Count)
            {
                Debug.LogWarning($"[cjh test] ShopModel.SetShopItemBySlotIndex() - 警告：槽位索引 {containerID} 越界");
                return;
            }

            // 获取该槽位当前的商品索引（循环）
            int itemIndex = _shopContainerItemIndex[containerID] % _shopItems.Count;
            int itemID = _shopItems[itemIndex].Id;
            _shopContainerItemID[containerID] = itemID;
            
            Debug.Log($"[cjh test] ShopModel.SetShopItemBySlotIndex() - 槽位 {containerID} 设置商品ID: {itemID} (商品索引: {itemIndex})");
        }

        /// <summary>
        /// 刷新槽位商品（购买后调用，槽位商品索引 +1）
        /// </summary>
        /// <param name="containerID">槽位索引</param>
        public void SetShopItem(int containerID)
        {
            if (_shopItems == null || _shopItems.Count == 0)
            {
                Debug.LogWarning($"[cjh test] ShopModel.SetShopItem() - 警告：商品列表为空，无法刷新槽位 {containerID}");
                return;
            }

            if (containerID < 0 || containerID >= _shopContainerItemID.Count)
            {
                Debug.LogWarning($"[cjh test] ShopModel.SetShopItem() - 警告：槽位索引 {containerID} 越界");
                return;
            }

            // 该槽位的商品索引 +1（购买后获取下一个商品）
            _shopContainerItemIndex[containerID]++;
            
            // 循环获取商品
            int itemIndex = _shopContainerItemIndex[containerID] % _shopItems.Count;
            int itemID = _shopItems[itemIndex].Id;
            _shopContainerItemID[containerID] = itemID;
            
            Debug.Log($"[cjh test] ShopModel.SetShopItem() - 槽位 {containerID} 刷新商品ID: {itemID} (商品索引: {_shopContainerItemIndex[containerID]} -> {itemIndex})");
        }

        /// <summary>
        /// 增加槽位的购买次数
        /// </summary>
        /// <param name="containerID">槽位索引</param>
        public void IncrementBuyCount(int containerID)
        {
            if (containerID >= 0 && containerID < _shopContainer.Count)
            {
                _shopContainer[containerID]++;
            }
        }

        #endregion

        #region 获取内部数据

        /// <summary>
        /// 获取槽位的购买次数
        /// </summary>
        /// <param name="container">槽位索引</param>
        /// <returns>购买次数</returns>
        public int GetShopContainerBuyCount(int container)
        {
            if (container < 0 || container >= _shopContainer.Count)
            {
                Debug.LogWarning($"槽位索引 {container} 越界");
                return 0;
            }
            return _shopContainer[container];
        }

        /// <summary>
        /// 获取槽位的商品ID
        /// </summary>
        /// <param name="container">槽位索引</param>
        /// <returns>商品ID，-1表示空</returns>
        public int GetShopContainerItemID(int container)
        {
            if (container < 0 || container >= _shopContainerItemID.Count)
            {
                Debug.LogWarning($"槽位索引 {container} 越界");
                return -1;
            }
            return _shopContainerItemID[container];
        }

        /// <summary>
        /// 获取关卡的商店配置
        /// </summary>
        /// <param name="shopID">关卡ID（对应商店ID）</param>
        /// <returns>商店配置</returns>
        public ShopConfig GetShopConfig(int shopID)
        {
            return this.GetUtility<ConfigUtility>().Config.TbShopConfig.Get(shopID);
        }

        /// <summary>
        /// 获取商品的物品配置
        /// </summary>
        /// <param name="itemID">商品ID</param>
        /// <returns>物品配置</returns>
        public ItemData GetItemConfig(int itemID)
        {
            return this.GetUtility<ConfigUtility>().Config.TbItemConfig.Get(itemID);
        }

        /// <summary>
        /// 获取商品的商店配置信息（包含价格）
        /// </summary>
        /// <param name="itemID">商品ID</param>
        /// <returns>商品商店配置</returns>
        public ItemShopData GetItemShopConfig(int itemID)
        {
            return this.GetUtility<ConfigUtility>().Config.TbItemShopConfig.Get(itemID);
        }

        /// <summary>
        /// 获取商品的基础价格（从 ItemShopConfig 获取）
        /// </summary>
        /// <param name="itemID">商品ID</param>
        /// <returns>基础价格</returns>
        public int GetItemBasePrice(int itemID)
        {
            try
            {
                var itemShopData = this.GetUtility<ConfigUtility>()
                    .Config
                    .TbItemShopConfig
                    .Get(itemID);

                return Mathf.RoundToInt(itemShopData.Price);
            }
            catch
            {
                Debug.LogWarning($"找不到商品ID {itemID} 的价格配置，返回默认价格100");
                return 100;  // 默认价格
            }
        }

        /// <summary>
        /// 根据购买次数计算实际价格
        /// </summary>
        /// <param name="containerID">槽位索引</param>
        /// <param name="basePrice">基础价格</param>
        /// <returns>实际价格</returns>
        public int GetItemPrice(int containerID, float basePrice)
        {
            int buyCount = GetShopContainerBuyCount(containerID);
            // 公式：基础价格 * (1 + 购买次数 * 涨价比例)
            return Mathf.RoundToInt(basePrice * (1 + buyCount * _upPriceRadio));
        }

        #endregion
    }
}