using UnityEngine;
using Script.Service.Interface;

namespace Script.Service.View.Component
{
    /// <summary>
    /// Sprite 专用拖拽组件
    /// 使用 OnMouseDown/OnMouseDrag/OnMouseUp 事件
    /// 适用于世界空间的 2D Sprite 对象
    /// </summary>
    public class DraggableSprite : MonoBehaviour, ICanDrag
    {
        [Header("拖拽轴设置")]
        [SerializeField] private bool canDragX = true;
        [SerializeField] private bool canDragY = true;
        
        [Header("位置限制")]
        [SerializeField] private bool useConstraints = false;
        [SerializeField] private Vector2 minPosition = new Vector2(-100, -100);
        [SerializeField] private Vector2 maxPosition = new Vector2(100, 100);
        
        [Header("拖拽设置")]
        [SerializeField] private float dragZ = 0f;  // 拖拽时的 Z 坐标
        [SerializeField] private bool handleRigidbody = true; // 是否自动处理 Rigidbody
        
        private Camera _mainCamera;
        private Vector3 _screenPoint;
        private Vector3 _offset;
        private bool _isDragging = false;
        private Rigidbody2D _rb;
        
        public bool IsDragging => _isDragging;
        public bool CanDragX { get => canDragX; set => canDragX = value; }
        public bool CanDragY { get => canDragY; set => canDragY = value; }

        private void Awake()
        {
            _mainCamera = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
            
            // 确保有 Collider
            if (GetComponent<Collider2D>() == null)
            {
                Debug.LogWarning($"[DraggableSprite] {gameObject.name} 需要 Collider2D！正在添加 BoxCollider2D...");
                gameObject.AddComponent<BoxCollider2D>();
            }
        }

        private void OnMouseDown()
        {
            if (!canDragX && !canDragY)
                return;

            _isDragging = true;
            
            // 处理 Rigidbody
            if (handleRigidbody && _rb != null)
            {
                _rb.isKinematic = true;
                _rb.velocity = Vector2.zero;
            }
            
            // 记录鼠标点击时的屏幕坐标和偏移
            _screenPoint = _mainCamera.WorldToScreenPoint(transform.position);
            _offset = transform.position - _mainCamera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));

            OnDragStart(Input.mousePosition);
            
            Debug.Log($"[DraggableSprite] 开始拖拽 {gameObject.name} - CanDragX: {canDragX}, CanDragY: {canDragY}");
        }

        private void OnMouseDrag()
        {
            if (!_isDragging || (!canDragX && !canDragY))
                return;

            // 获取鼠标在世界坐标中的位置
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            Vector3 cursorPosition = _mainCamera.ScreenToWorldPoint(cursorPoint) + _offset;
            
            Vector3 currentPosition = transform.position;

            // 根据拖拽轴限制
            float newX = canDragX ? cursorPosition.x : currentPosition.x;
            float newY = canDragY ? cursorPosition.y : currentPosition.y;
            float newZ = dragZ != 0 ? dragZ : currentPosition.z;

            // 应用位置限制
            if (useConstraints)
            {
                newX = Mathf.Clamp(newX, minPosition.x, maxPosition.x);
                newY = Mathf.Clamp(newY, minPosition.y, maxPosition.y);
            }

            transform.position = new Vector3(newX, newY, newZ);
            
            OnDragging(Input.mousePosition);
        }

        private void OnMouseUp()
        {
            if (!_isDragging)
                return;

            _isDragging = false;
            
            // 处理 Rigidbody
            if (handleRigidbody && _rb != null)
            {
                _rb.isKinematic = false;
            }
            
            OnDragEnd(Input.mousePosition);
            
            Debug.Log($"[DraggableSprite] 结束拖拽 {gameObject.name}");
        }

        // ICanDrag 接口实现
        public virtual void OnDragStart(Vector2 position)
        {
            // 可以被子类重写
        }

        public virtual void OnDragging(Vector2 position)
        {
            // 可以被子类重写
        }

        public virtual void OnDragEnd(Vector2 position)
        {
            // 可以被子类重写
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
