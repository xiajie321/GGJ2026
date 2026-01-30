using System.Threading;
using Cysharp.Threading.Tasks;
using QFramework;
using Script.Service.Architecture;
using Script.Service.System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Service.View.UI.Panel
{
    public class UIMiniMapData : UIPanelData
    {
        public float CameraViewRatio = 0.2f;
    }
    
    public partial class UIMiniMap : UIPanel, IController, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Camera _mainCamera;
        private CameraEdgeScrollingSystem _cameraSystem;

        private float _minimapWidth;
        private float _cameraRectWidth;

        private CancellationTokenSource _cts;
        private EventTrigger _areaEventTrigger;
        
        private bool _isDragging = false;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIMiniMapData ?? new UIMiniMapData();

            _mainCamera = Camera.main;
            _cameraSystem = this.GetSystem<CameraEdgeScrollingSystem>();
            
            _minimapWidth = Area.rectTransform.rect.width;
            _cameraRectWidth = _minimapWidth * Data.CameraViewRatio;
            
            // 设置相机视野矩形样式
            CameraViewRect.color = new Color(0f, 1f, 0f, 0.3f);
            CameraViewRect.rectTransform.sizeDelta = new Vector2(
                _cameraRectWidth, 
                Area.rectTransform.rect.height
            );
            
            // 确保 CameraViewRect 可以接收射线检测
            if (CameraViewRect.raycastTarget == false)
            {
                CameraViewRect.raycastTarget = true;
                Debug.Log("[MiniMap] 已启用 CameraViewRect 的 Raycast Target");
            }
            
            // 添加点击监听到 Area
            AddClickListener();
            
            Debug.Log($"[MiniMap] 初始化完成 - 小地图宽度: {_minimapWidth}px, 比例: {Data.CameraViewRatio:P0}, 矩形宽度: {_cameraRectWidth}px");
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            _cts = new CancellationTokenSource();
            UpdateLoop(_cts.Token).Forget();
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            
            RemoveClickListener();
        }

        /// <summary>
        /// 添加点击监听到 Area
        /// </summary>
        private void AddClickListener()
        {
            _areaEventTrigger = Area.GetComponent<EventTrigger>();
            if (_areaEventTrigger == null)
            {
                _areaEventTrigger = Area.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(OnAreaClick);
            _areaEventTrigger.triggers.Add(entry);
            
            Debug.Log("[MiniMap] Area 点击监听已添加");
        }

        /// <summary>
        /// 移除点击监听
        /// </summary>
        private void RemoveClickListener()
        {
            if (_areaEventTrigger != null)
            {
                _areaEventTrigger.triggers.Clear();
            }
        }

        /// <summary>
        /// 开始拖拽（IBeginDragHandler）
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            Debug.Log($"[MiniMap] 开始拖拽相机矩形 - 位置: {eventData.position}");
        }

        /// <summary>
        /// 拖拽中（IDragHandler）
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging || _cameraSystem == null)
            {
                Debug.LogWarning($"[MiniMap] 拖拽被跳过 - isDragging: {_isDragging}, cameraSystem: {_cameraSystem != null}");
                return;
            }

            // 获取拖拽位置在 Area 中的本地坐标
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Area.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
            {
                Debug.Log($"[MiniMap] 拖拽中 - 本地X: {localPoint.x:F1}");
                MoveCameraByMinimapPosition(localPoint.x);
            }
            else
            {
                Debug.LogWarning("[MiniMap] 无法转换拖拽坐标");
            }
        }

        /// <summary>
        /// 结束拖拽（IEndDragHandler）
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            Debug.Log("[MiniMap] 结束拖拽");
        }

        /// <summary>
        /// Area 点击事件
        /// </summary>
        private void OnAreaClick(BaseEventData eventData)
        {
            // 如果正在拖拽，不响应点击
            if (_isDragging)
                return;

            var pointerData = eventData as PointerEventData;
            if (pointerData == null)
                return;

            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Area.rectTransform,
                pointerData.position,
                pointerData.pressEventCamera,
                out localPoint))
            {
                Debug.Log($"[MiniMap] Area 点击 - 本地X: {localPoint.x:F1}");
                MoveCameraByMinimapPosition(localPoint.x);
            }
        }

        /// <summary>
        /// 根据小地图位置移动相机
        /// </summary>
        private void MoveCameraByMinimapPosition(float minimapLocalX)
        {
            if (_cameraSystem == null)
            {
                Debug.LogWarning("[MiniMap] CameraSystem 为空");
                return;
            }

            var sceneBounds = _cameraSystem.GetSceneBounds();
            float sceneWidth = sceneBounds.y - sceneBounds.x;
            
            if (sceneWidth <= 0)
            {
                Debug.LogWarning($"[MiniMap] 场景宽度无效: {sceneWidth}");
                return;
            }

            // 小地图坐标 → 归一化位置 (0-1)
            float normalizedX = (minimapLocalX / _minimapWidth) + 0.5f;

            // 归一化位置 → 世界坐标
            float targetWorldX = Mathf.Lerp(sceneBounds.x, sceneBounds.y, normalizedX);

            Debug.Log($"[MiniMap] 移动相机 - 小地图X: {minimapLocalX:F1}, 归一化: {normalizedX:F2}, 世界X: {targetWorldX:F1}");

            // 移动相机
            _cameraSystem.MoveCameraToWorldX(targetWorldX);
        }

        /// <summary>
        /// 更新循环
        /// </summary>
        private async UniTaskVoid UpdateLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                
                // 始终更新矩形位置，无论是否在拖拽
                UpdateCameraPosition();
            }
        }

        /// <summary>
        /// 更新相机矩形位置
        /// </summary>
        private void UpdateCameraPosition()
        {
            if (_mainCamera == null || _cameraSystem == null || CameraViewRect == null)
                return;

            var sceneBounds = _cameraSystem.GetSceneBounds();
            float sceneWidth = sceneBounds.y - sceneBounds.x;

            if (sceneWidth <= 0)
                return;

            float normalizedPos = Mathf.InverseLerp(
                sceneBounds.x,
                sceneBounds.y,
                _mainCamera.transform.position.x
            );

            float minimapX = (normalizedPos - 0.5f) * _minimapWidth;
            CameraViewRect.rectTransform.anchoredPosition = new Vector2(minimapX, 0);
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}