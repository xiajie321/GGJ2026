using System.Collections;
using System.Collections.Generic;
using QFramework;
using Script.Service.Model;
using Script.Service.Event;
using UnityEngine;

namespace Script.Service.System
{
    public class ShopSystem : AbstractSystem
    {
        private ShopModel _shopModel;
        private LevelModel _levelModel;

        protected override void OnInit()
        {
            _shopModel = this.GetModel<ShopModel>();
            _levelModel = this.GetModel<LevelModel>();
        }

        /// <summary>
        /// 初始化商店（游戏开始时调用）
        /// </summary>
        public void InitShop()
        {
            _shopModel.InitShopContainer();
            _shopModel.ResetShopContainer(_levelModel.LevelID);
        }

        /// <summary>
        /// 刷新所有商店槽位（不清零购买次数）
        /// </summary>
        public void RefreshAllShop()
        {
            _shopModel.ResetShopContainer(_levelModel.LevelID);
        }

        /// <summary>
        /// 刷新单个槽位（使用顺序刷新）
        /// </summary>
        /// <param name="slotIndex">槽位索引</param>
        public void RefreshSingleSlot(int slotIndex)
        {
            _shopModel.SetShopItem(slotIndex);
        }

        /// <summary>
        /// 购买商品
        /// </summary>
        /// <param name="shopContainer">槽位索引</param>
        public bool BuyShopItem(int shopContainer)
        {
            // 获取槽位商品ID
            int itemID = _shopModel.GetShopContainerItemID(shopContainer);

            // 检查槽位是否有商品
            if (itemID < 0)
            {
                this.GetSystem<MessageTipSystem>().ShowTip("该槽位没有商品");
                return false;
            }

            // 获取商品基础价格
            int basePrice = _shopModel.GetItemBasePrice(itemID);

            // 根据购买次数计算实际价格
            int actualPrice = _shopModel.GetItemPrice(shopContainer, basePrice);

            // 检查是否可以购买
            if (!CheckCanBuy(shopContainer, actualPrice))
            {
                this.GetSystem<MessageTipSystem>().ShowTip("金钱不足");
                return false;
            }

            // 扣除金钱
            if (!_levelModel.SubMoney(actualPrice))
            {
                this.GetSystem<MessageTipSystem>().ShowTip("扣款失败");
                return false;
            }

            // 增加购买次数
            _shopModel.IncrementBuyCount(shopContainer);

            // 刷新该槽位（顺序刷新）
            _shopModel.SetShopItem(shopContainer);

            // 发送购买成功事件
            this.SendEvent(new ShopItemBoughtEvent
            {
                ItemID = itemID,
                SlotIndex = shopContainer,
                Price = actualPrice,
                RemainingMoney = _levelModel.Money
            });

            Debug.Log($"[ShopSystem] 购买成功！商品ID: {itemID}, 花费: {actualPrice}, 剩余金钱: {_levelModel.Money}");
            
            return true;
        }

        /// <summary>
        /// 检查是否可以购买
        /// </summary>
        /// <param name="shopContainer">槽位索引</param>
        /// <param name="actualPrice">实际价格</param>
        /// <returns>是否可以购买</returns>
        private bool CheckCanBuy(int shopContainer, int actualPrice)
        {
            return _levelModel.CanAfford(actualPrice);
        }
    }
}