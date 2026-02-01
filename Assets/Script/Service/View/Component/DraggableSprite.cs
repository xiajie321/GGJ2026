using UnityEngine;
using Script.Service.Interface;
using Script.Service.Utility;
using Script.SODataScript.TbConfig;

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

        [Header("碰撞体设置")]
        [SerializeField] private bool autoFitCollider = true; // 是否自动调整碰撞体大小以包裹 Sprite
        
        private Camera _mainCamera;
        private Vector3 _screenPoint;
        private Vector3 _offset;
        private bool _isDragging = false;
        private Rigidbody2D _rb;
        private Vector3 _targetPosition;
        private float _moveSpeed = 5f;
        
        public bool IsDragging => _isDragging;
        public bool CanDragX { get => canDragX; set => canDragX = value; }
        public bool CanDragY { get => canDragY; set => canDragY = value; }

        private void Awake()
        {
            _mainCamera = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
            
            // 自动适配 Collider
            if (autoFitCollider)
            {
                UpdateColliderSize();
            }
            else
            {
                // 原有逻辑：仅当没有任何碰撞体时添加
                if (GetComponent<Collider2D>() == null)
                {
                    Debug.LogWarning($"[DraggableSprite] {gameObject.name} 需要 Collider2D！正在添加 BoxCollider2D...");
                    gameObject.AddComponent<BoxCollider2D>();
                }
            }
        }

        /// <summary>
        /// 自动更新碰撞体大小以包裹精灵
        /// 支持 Simple 和 Sliced 模式，自动处理 Pivot 偏移
        /// </summary>
        [ContextMenu("Fit Collider To Sprite")]
        public void UpdateColliderSize()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            
            // 如果没有 SpriteRenderer，无法计算，仅做保底处理
            if (spriteRenderer == null || spriteRenderer.sprite == null)
            {
                if (GetComponent<Collider2D>() == null)
                {
                    gameObject.AddComponent<BoxCollider2D>();
                }
                return;
            }
            
            var boxCollider = GetComponent<BoxCollider2D>();
            
            if (boxCollider == null)
            {
                if (GetComponent<Collider2D>() == null)
                {
                    boxCollider = gameObject.AddComponent<BoxCollider2D>();
                }
                else
                {
                    // 如果已经有其他形状的碰撞体（如圆形、多边形），，直接返回
                    return;
                }
            }

            // 根据 Sprite 模式计算大小和偏移
            if (spriteRenderer.drawMode == SpriteDrawMode.Simple)
            {
                // Simple 模式：直接使用 sprite 的 bounds（包含 Pivot 偏移信息）
                boxCollider.size = spriteRenderer.sprite.bounds.size;
                boxCollider.offset = spriteRenderer.sprite.bounds.center;
            }
            else
            {
                // Sliced / Tiled 模式：使用 SpriteRenderer 的 size
                boxCollider.size = spriteRenderer.size;

                // 计算 Pivot 带来的中心偏移
                // Pivot (0.5, 0.5) -> Offset (0, 0)
                // Pivot (0, 0) -> Offset (size/2, size/2)
                float pivotX = spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.rect.width;
                float pivotY = spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.rect.height;

                boxCollider.offset = new Vector2(
                    (0.5f - pivotX) * spriteRenderer.size.x,
                    (0.5f - pivotY) * spriteRenderer.size.y
                );
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

            GetMoveSpeed();

            OnDragStart(Input.mousePosition);
            
            Debug.Log($"[DraggableSprite] 开始拖拽 {gameObject.name}");
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

            _targetPosition = new Vector3(newX, newY, newZ);
            
            if (_isDragging)
            {
                // 获取重力值
                float gravity = 0f;
                var itemController = GetComponent<Script.Service.View.Game.ItemControllerMono>();
                if (itemController != null && itemController.ItemData != null)
                {
                    gravity = itemController.ItemData.Gravity;
                }

                // 计算移动方向
                Vector3 direction = _targetPosition - transform.position;
                float distance = direction.magnitude;
                if (distance > 0.01f) // 避免除以零
                {
                    direction.Normalize();

                    // 根据重力和移动方向调整速度
                    float adjustedSpeed = _moveSpeed;
                    if (gravity > 0)
                    {
                        // 向下移动更快，向上移动更慢
                        if (direction.y < 0) // 向下移动
                        {
                            adjustedSpeed *= (1 + gravity * 0.5f);
                        }
                        else if (direction.y > 0) // 向上移动
                        {
                            adjustedSpeed *= (1 - gravity * 0.3f);
                            // 确保速度不会过低
                            adjustedSpeed = Mathf.Max(adjustedSpeed, _moveSpeed * 0.5f);
                        }
                    }

                    // 使用线性插值平滑移动到目标位置
                    transform.position = Vector3.Lerp(transform.position, _targetPosition, adjustedSpeed * Time.deltaTime);
                }
            }
            
            OnDragging(Input.mousePosition);
        }

        private void Update()
        {

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
        public virtual void OnDragStart(Vector2 position) { }
        public virtual void OnDragging(Vector2 position) { }
        public virtual void OnDragEnd(Vector2 position) { }

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

        /// <summary>
        /// 获取Item等级与对应移动速度
        /// </summary>
        public void GetMoveSpeed()
        {
            // 获取Item等级和对应移动速度
            var itemController = GetComponent<Script.Service.View.Game.ItemControllerMono>();
            if (itemController != null && itemController.ItemData != null)
            {
                var configUtility = itemController.GetArchitecture().GetUtility<ConfigUtility>();
                if (configUtility != null && configUtility.Config != null && configUtility.Config.TbLevelConfig != null)
                {
                    // 假设当前是第一关，获取第一关的配置
                    var levelData = configUtility.Config.TbLevelConfig.Get(0);
                    if (levelData != null)
                    {
                        switch (itemController.ItemData.Level)
                        {
                            case ItemLevel.S:
                                _moveSpeed = levelData.SMoveSpeed;
                                break;
                            case ItemLevel.A:
                                _moveSpeed = levelData.AMoveSpeed;
                                break;
                            case ItemLevel.B:
                                _moveSpeed = levelData.BMoveSpeed;
                                break;
                            default:
                                _moveSpeed = 5f; // 默认速度
                                break;
                        }
                    }
                }
            }
        }
    }
}