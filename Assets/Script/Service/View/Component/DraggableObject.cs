using UnityEngine;
using UnityEngine.EventSystems;
using Script.Service.Interface;

namespace Script.Service.View.Component
{
    /// <summary>
    /// 通用拖拽组件
    /// 可用于 UI 元素（RectTransform）或游戏对象（Transform）
    /// </summary>
    public class DraggableObject : MonoBehaviour, ICanDrag, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("拖拽轴设置")]
        [SerializeField] private bool canDragX = true;
        [SerializeField] private bool canDragY = true;
        
        [Header("位置限制")]
        [SerializeField] private bool useConstraints = false;
        [SerializeField] private Vector2 minPosition = new Vector2(-1000, -1000);
        [SerializeField] private Vector2 maxPosition = new Vector2(1000, 1000);
        
        [Header("其他设置")]
        [SerializeField] private bool dragInScreenSpace = true;  // 是否在屏幕空间拖拽（用于世界对象）
        [SerializeField] private Canvas canvas;  // UI 元素需要指定 Canvas
        
        private RectTransform _rectTransform;
        private Vector2 _dragOffset;
        private bool _isDragging = false;
        
        public bool CanDragX { get => canDragX; set => canDragX = value; }
        public bool CanDragY { get => canDragY; set => canDragY = value; }
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            // 如果是 UI 元素
            if (_rectTransform != null)
            {
                // 如果没有指定 Canvas，尝试自动查找
                if (canvas == null)
                {
                    canvas = GetComponentInParent<Canvas>();
                }
            }
            else
            {
                // 如果是世界空间对象，确保有 Collider
                var collider2D = GetComponent<Collider2D>();
                if (collider2D == null)
                {
                    Debug.LogWarning($"[DraggableObject] {gameObject.name} 需要 Collider2D 组件才能在世界空间中拖拽！正在自动添加 BoxCollider2D...");
                    gameObject.AddComponent<BoxCollider2D>();
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!canDragX && !canDragY)
                return;
                
            _isDragging = true;
            
            // 计算拖拽偏移（UI 元素）
            if (_rectTransform != null)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _rectTransform.parent as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out localPoint);
                    
                _dragOffset = _rectTransform.anchoredPosition - localPoint;
            }
            
            OnDragStart(eventData.position);
            
            Debug.Log($"[Drag] 开始拖拽 {gameObject.name} - CanDragX: {canDragX}, CanDragY: {canDragY}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging || (!canDragX && !canDragY))
                return;

            if (_rectTransform != null)
            {
                // UI 拖拽（RectTransform）
                DragUIObject(eventData);
            }
            else
            {
                // 世界空间拖拽（Transform）
                DragWorldObject(eventData);
            }
            
            OnDragging(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging)
                return;
                
            _isDragging = false;
            OnDragEnd(eventData.position);
            
            Debug.Log($"[Drag] 结束拖拽 {gameObject.name}");
        }

        /// <summary>
        /// UI 对象拖拽
        /// </summary>
        private void DragUIObject(PointerEventData eventData)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
            {
                Vector2 targetPosition = localPoint + _dragOffset;
                Vector2 currentPosition = _rectTransform.anchoredPosition;

                // 根据拖拽轴限制
                float newX = canDragX ? targetPosition.x : currentPosition.x;
                float newY = canDragY ? targetPosition.y : currentPosition.y;

                // 应用位置限制
                if (useConstraints)
                {
                    newX = Mathf.Clamp(newX, minPosition.x, maxPosition.x);
                    newY = Mathf.Clamp(newY, minPosition.y, maxPosition.y);
                }

                _rectTransform.anchoredPosition = new Vector2(newX, newY);
            }
        }

        /// <summary>
        /// 世界空间对象拖拽
        /// </summary>
        private void DragWorldObject(PointerEventData eventData)
        {
            Vector3 worldPosition;
            
            if (dragInScreenSpace)
            {
                // 屏幕空间转世界空间
                Vector3 screenPoint = new Vector3(eventData.position.x, eventData.position.y, 
                    Camera.main.WorldToScreenPoint(transform.position).z);
                worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);
            }
            else
            {
                // 射线检测平面
                Plane dragPlane = new Plane(Vector3.forward, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(eventData.position);
                
                if (dragPlane.Raycast(ray, out float distance))
                {
                    worldPosition = ray.GetPoint(distance);
                }
                else
                {
                    return;
                }
            }

            Vector3 currentPosition = transform.position;

            // 根据拖拽轴限制
            float newX = canDragX ? worldPosition.x : currentPosition.x;
            float newY = canDragY ? worldPosition.y : currentPosition.y;

            // 应用位置限制
            if (useConstraints)
            {
                newX = Mathf.Clamp(newX, minPosition.x, maxPosition.x);
                newY = Mathf.Clamp(newY, minPosition.y, maxPosition.y);
            }

            transform.position = new Vector3(newX, newY, currentPosition.z);
        }

        // ICanDrag 接口实现（可以被子类重写）
        public virtual void OnDragStart(Vector2 position)
        {
            // 子类可以重写这个方法添加自定义行为
        }

        public virtual void OnDragging(Vector2 position)
        {
            // 子类可以重写这个方法添加自定义行为
        }

        public virtual void OnDragEnd(Vector2 position)
        {
            // 子类可以重写这个方法添加自定义行为
        }

        /// <summary>
        /// 设置拖拽轴
        /// </summary>
        public void SetDragAxis(bool dragX, bool dragY)
        {
            canDragX = dragX;
            canDragY = dragY;
        }

        /// <summary>
        /// 设置位置限制
        /// </summary>
        public void SetConstraints(Vector2 min, Vector2 max, bool enable = true)
        {
            minPosition = min;
            maxPosition = max;
            useConstraints = enable;
        }
    }
}
