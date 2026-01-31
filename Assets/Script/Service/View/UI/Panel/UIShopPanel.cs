using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QFramework;
using Script.Service.System;
using Script.Service.Event;
using Script.Service.Architecture;
using Script.Service.Model;
using TMPro;

namespace Service.View.UI.Panel
{
    public class UIShopPanelData : UIPanelData
    {
    }

    public partial class UIShopPanel : UIPanel,IController
    {
        // 拖拽预览对象
        private GameObject _dragPreviewObject;
        private Canvas _rootCanvas;
        
        // 最后拖拽释放的世界坐标
        private Vector3 _lastDropWorldPosition;

        private ShopSystem _shopSystem;
        private ShopModel _shopModel;

        private GameObject[] _shopContainerGo = new GameObject[6];
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIShopPanelData ?? new UIShopPanelData();
            _shopSystem = this.GetSystem<ShopSystem>();
            _shopModel = this.GetModel<ShopModel>();
            // 获取根 Canvas（用于放置拖拽预览）
            _rootCanvas = GetComponentInParent<Canvas>().rootCanvas;

            // 为 Shop_1 到 Shop_6 添加拖拽功能
            AddDragHandler(Shop_1, 1);
            AddDragHandler(Shop_2, 2);
            AddDragHandler(Shop_3, 3);
            AddDragHandler(Shop_4, 4);
            AddDragHandler(Shop_5, 5);
            AddDragHandler(Shop_6, 6);
            
            //0~5 缓存Shop_X  方便后续查找
            _shopContainerGo[0] = Shop_1.gameObject;
            _shopContainerGo[1] = Shop_2.gameObject;
            _shopContainerGo[2] = Shop_3.gameObject;
            _shopContainerGo[3] = Shop_4.gameObject;
            _shopContainerGo[4] = Shop_5.gameObject;
            _shopContainerGo[5] = Shop_6.gameObject;

            // 监听购买成功事件
            this.RegisterEvent<ShopItemBoughtEvent>(OnShopItemBought);
            
            IllustratedGuideBtn.onClick.AddListener(() =>
            {
                //切换 LevelModel 状态为 Playing
                this.GetModel<LevelModel>().EnterPlayingState();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            // 初始化所有商店槽位的显示
            for (int i = 0; i < 6; i++)
            {
                SetShopItem(i);
            }

            //切换LevelSystem状态为Playing
            
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
            // 注销购买事件监听
            this.UnRegisterEvent<ShopItemBoughtEvent>(OnShopItemBought);
        }


        /// 设置商店槽位商品
        public void SetShopItem(int shopIndex)
        {
            Debug.Log($"[cjh test] UIShopPanel.SetShopItem() - 设置槽位 {shopIndex}");
            
            //获取商店槽位商品
            var shopItemID = _shopModel.GetShopContainerItemID(shopIndex);
            
            if (shopItemID < 0)
            {
                Debug.LogWarning($"[cjh test] UIShopPanel.SetShopItem() - 警告：槽位 {shopIndex} 没有商品");
                return;
            }

            //获取商店物品配置
            var shopItemConfig = _shopModel.GetItemShopConfig(shopItemID);
            //获取物品配置
            var itemConfig = _shopModel.GetItemConfig(shopItemID);

            //获取价格
            var price = _shopModel.GetItemPrice(shopIndex, shopItemConfig.Price);
            
            // 设置图标
            Transform iconTransform = _shopContainerGo[shopIndex].transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null)
                {
                    iconImage.sprite = itemConfig.Sprite;
                }
            }

            // 设置价格文本
            Transform priceTransform = _shopContainerGo[shopIndex].transform.Find("Price");
            if (priceTransform != null)
            {
                Transform textTransform = priceTransform.Find("Text (TMP)");
                if (textTransform != null)
                {
                    TextMeshProUGUI priceText = textTransform.GetComponent<TextMeshProUGUI>();
                    if (priceText != null)
                    {
                        priceText.text = price.ToString();
                    }
                }

            }

        }
        
        /// <summary>
        /// 为 Shop Image 添加拖拽事件监听
        /// </summary>
        private void AddDragHandler(Image shopImage, int shopIndex)
        {
            // 添加 EventTrigger 组件
            EventTrigger trigger = shopImage.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = shopImage.gameObject.AddComponent<EventTrigger>();
            }

            // 开始拖拽
            EventTrigger.Entry beginDragEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.BeginDrag
            };
            beginDragEntry.callback.AddListener((data) =>
            {
                OnBeginDrag((PointerEventData)data, shopImage, shopIndex);
            });
            trigger.triggers.Add(beginDragEntry);

            // 拖拽中
            EventTrigger.Entry dragEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Drag
            };
            dragEntry.callback.AddListener((data) =>
            {
                OnDrag((PointerEventData)data);
            });
            trigger.triggers.Add(dragEntry);

            // 结束拖拽
            EventTrigger.Entry endDragEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.EndDrag
            };
            endDragEntry.callback.AddListener((data) =>
            {
                OnEndDrag((PointerEventData)data, shopIndex);
            });
            trigger.triggers.Add(endDragEntry);
        }

        #region 拖拽事件处理

        /// <summary>
        /// 获取 Shop 下的 Icon 图标
        /// </summary>
        private Image GetIconImage(Image shopImage)
        {
            // 在 Shop_X 的子对象中查找名为 "Icon" 的对象
            Transform iconTransform = shopImage.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null)
                {
                    return iconImage;
                }
            }

            // 如果没找到 Icon，返回 shopImage 本身
            Debug.LogWarning($"[UIShopPanel] 未找到 {shopImage.name} 下的 Icon 子对象，使用 Shop 本身的图标");
            return shopImage;
        }

        /// <summary>
        /// 开始拖拽
        /// </summary>
        private void OnBeginDrag(PointerEventData eventData, Image sourceImage, int shopIndex)
        {
            Debug.Log($"[UIShopPanel] 开始拖拽 Shop_{shopIndex}");

            // 获取 Icon 图标
            Image iconImage = GetIconImage(sourceImage);

            // 创建拖拽预览对象
            _dragPreviewObject = new GameObject($"DragPreview_Shop_{shopIndex}");
            _dragPreviewObject.transform.SetParent(_rootCanvas.transform, false);

            // 添加 Image 组件并复制 Icon 的图标
            Image previewImage = _dragPreviewObject.AddComponent<Image>();
            previewImage.sprite = iconImage.sprite;  // ← 使用 Icon 的 sprite
            previewImage.raycastTarget = false;  // 不阻挡射线检测

            // 设置半透明
            Color transparentColor = iconImage.color;
            transparentColor.a = 0.5f;  // 50% 透明度
            previewImage.color = transparentColor;

            // 设置为原始大小（显示道具的实际大小）
            previewImage.SetNativeSize();

            // 设置为最上层
            _dragPreviewObject.transform.SetAsLastSibling();

            // 初始位置设置为鼠标位置
            _dragPreviewObject.transform.position = eventData.position;
        }

        /// <summary>
        /// 拖拽中
        /// </summary>
        private void OnDrag(PointerEventData eventData)
        {
            if (_dragPreviewObject != null)
            {
                // 更新预览对象位置，跟随鼠标
                _dragPreviewObject.transform.position = eventData.position;
            }
        }

        /// <summary>
        /// 结束拖拽
        /// </summary>
        private void OnEndDrag(PointerEventData eventData, int shopIndex)
        {
            Debug.Log($"[UIShopPanel] 结束拖拽 Shop_{shopIndex}，鼠标位置: {eventData.position}");

            if (_dragPreviewObject != null)
            {
                // 销毁预览对象
                Destroy(_dragPreviewObject);
                _dragPreviewObject = null;

                // 转换屏幕坐标为世界坐标
                Vector3? worldPos = GetWorldPosition(eventData.position);
                
                if (worldPos.HasValue)
                {
                    Debug.Log($"[UIShopPanel] 世界坐标: {worldPos.Value}");
                    
                    // 保存世界坐标，供购买成功事件使用
                    _lastDropWorldPosition = worldPos.Value;
                    
                    // 调用购买逻辑（ShopSystem会自动发送事件）
                    _shopSystem.BuyShopItem(shopIndex - 1);
                }
                else
                {
                    Debug.LogWarning("[UIShopPanel] 无法获取世界坐标，取消购买");
                }
            }
        }
        
        /// <summary>
        /// 将屏幕坐标转换为世界坐标（适用于2D正交摄像机）
        /// </summary>
        private Vector3? GetWorldPosition(Vector2 screenPosition)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("[UIShopPanel] 找不到主摄像机");
                return null;
            }

            // 对于2D正交摄像机，设置正确的深度
            Vector3 screenPos = new Vector3(screenPosition.x, screenPosition.y, -mainCamera.transform.position.z);
            
            // 转换为世界坐标
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;  // 2D 游戏物体通常在 Z=0 平面
            
            return worldPos;
        }
        
        /// <summary>
        /// 响应购买成功事件
        /// </summary>
        private void OnShopItemBought(ShopItemBoughtEvent evt)
        {
            Camera mainCamera = Camera.main;
            Debug.Log($"[UIShopPanel] 商品购买成功！槽位: {evt.SlotIndex}, 物品ID: {evt.ItemID}, 花费: {evt.Price}");
            
            // 显示购买成功提示
            this.GetSystem<MessageTipSystem>().ShowTip($"购买成功！花费 {evt.Price}", mainCamera.WorldToScreenPoint(_lastDropWorldPosition));
            
            // 在世界坐标生成物品
            SpawnItemInWorld(evt.ItemID, _lastDropWorldPosition);
            
            // 刷新对应槽位的显示（显示新商品和更新价格）
            SetShopItem(evt.SlotIndex);
            
            // TODO: 其他UI更新
            // - 播放购买音效
            // - 显示购买特效
            // - 更新金钱显示
        }
        
        /// <summary>
        /// 在世界坐标生成物品
        /// </summary>
        private void SpawnItemInWorld(int itemID, Vector3 worldPosition)
        {
            Debug.Log($"[UIShopPanel] 在世界坐标 {worldPosition} 生成物品ID: {itemID}");
            
            // 通过 FactorySystem 获取 ItemFactory 生成物品
            var itemController = this.GetSystem<FactorySystem>().ItemFactory.Get(itemID);
            
            if (itemController != null)
            {
                // 设置物品位置
                itemController.transform.position = worldPosition;
                Debug.Log($"[UIShopPanel] 物品生成成功，位置: {worldPosition}");
            }
            else
            {
                Debug.LogError($"[UIShopPanel] 物品生成失败，ItemID: {itemID}");
            }
        }

        public IArchitecture GetArchitecture()
        {
              return GameArchitecture.Interface;
        }

        #endregion
    }
}