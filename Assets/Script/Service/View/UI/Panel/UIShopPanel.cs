using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QFramework;

namespace Service.View.UI.Panel
{
    public class UIShopPanelData : UIPanelData
    {
    }

    public partial class UIShopPanel : UIPanel
    {
        // 拖拽预览对象
        private GameObject _dragPreviewObject;
        private Canvas _rootCanvas;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIShopPanelData ?? new UIShopPanelData();

            // 获取根 Canvas（用于放置拖拽预览）
            _rootCanvas = GetComponentInParent<Canvas>().rootCanvas;

            // 为 Shop_1 到 Shop_6 添加拖拽功能
            AddDragHandler(Shop_1, 1);
            AddDragHandler(Shop_2, 2);
            AddDragHandler(Shop_3, 3);
            AddDragHandler(Shop_4, 4);
            AddDragHandler(Shop_5, 5);
            AddDragHandler(Shop_6, 6);
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
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

                // TODO: 下一步 - 记录世界坐标并生成物品
                // Vector3 worldPos = GetWorldPosition(eventData.position);
                // Debug.Log($"世界坐标: {worldPos}");
            }
        }

        #endregion
    }
}