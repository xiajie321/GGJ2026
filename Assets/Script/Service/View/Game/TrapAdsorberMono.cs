using System;
using QFramework;
using UnityEngine;

namespace Script.Service.View.Game
{
    public class TrapAdsorberMono:MonoBehaviour
    {
        private bool _isJudgment;
        private ItemControllerMono _currentItem;
        private TrapControllerMono _trapController;
        private Collider2D _collider;
        
        public bool IsJudgment => _isJudgment;//有媳妇对象
        public ItemControllerMono ItemControllerMono => _currentItem;
        public TrapControllerMono TrapControllerMono => _trapController;

        private void Start()
        {
            _trapController = GetComponent<TrapControllerMono>();
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag.Equals("Trigger")) return;
            
            // 修复：如果记录有对象但对象已销毁，重置状态
            if (_isJudgment && _currentItem == null)
            {
                _isJudgment = false;
            }
            
            if(_isJudgment) return;
            
            var item = other.GetComponent<ItemControllerMono>();
            if (item == null) return;
            if(item.ItemData.Height != _trapController.TrapData.Height) return;
            
            _isJudgment = true;
            _currentItem = item;
            _currentItem.Parent(transform);
            
            // 如果正在拖拽，不要强制位置，也不要强制 Kinematic（因为 DraggableSprite 已经处理了）
            // 只有非拖拽状态下（例如生成的物品掉进陷阱？）才需要强制
            bool isDragging = _currentItem.DraggableSprite != null && _currentItem.DraggableSprite.IsDragging;
            
            if (!isDragging)
            {
                _currentItem.transform.localPosition = new Vector3(0, 0, -0.1f);
                
                if (_currentItem.Rigidbody2D != null)
                {
                    _currentItem.Rigidbody2D.isKinematic = true;
                    _currentItem.Rigidbody2D.velocity = Vector2.zero;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.tag.Equals("Trigger")) return;
            if(!_isJudgment) return;
            
            if (_currentItem != null && _currentItem.gameObject == other.gameObject)
            {
                // 移除高度检查，只要离开了就应该断开
                ReleaseItem();
            }
        }
        
        private void ReleaseItem()
        {
            _isJudgment = false;
            if (_currentItem != null)
            {
                if (gameObject.activeInHierarchy && _currentItem.gameObject.activeInHierarchy)
                {
                    _currentItem.transform.SetParent(null);
                    
                    // 恢复物理状态
                    // 如果正在拖拽，由 DraggableSprite 在松手时负责恢复，这里不干涉
                    bool isDragging = _currentItem.DraggableSprite != null && _currentItem.DraggableSprite.IsDragging;
                    if (!isDragging && _currentItem.Rigidbody2D != null)
                    {
                        _currentItem.Rigidbody2D.isKinematic = false;
                    }
                }
                _currentItem = null;
            }
        }

        private void Update()
        {
            // 1. 检查对象是否被销毁
            if (_isJudgment && _currentItem == null)
            {
                _isJudgment = false;
                return;
            }
            
            if(!_isJudgment || _currentItem == null) return;
            
            // 2. 检查父物体是否不再是当前陷阱（被其他陷阱吸附）
            if (_currentItem.transform.parent != transform)
            {
                _isJudgment = false;
                _currentItem = null;
                return;
            }

            // 3. 处理拖拽
            if(_currentItem.DraggableSprite != null && _currentItem.DraggableSprite.IsDragging) 
            {
                // 修复：如果拖拽出了范围但 OnTriggerExit2D 没触发（快速移动），手动断开
                // 使用距离检测代替 IsTouching，因为 IsTouching 在快速移动时可能不稳定
                // 假设陷阱半径约为 0.5 (根据 Sprite 大小调整)，物品半径约为 0.5
                // 阈值设为 1.5f 比较宽松，避免误判
                if (Vector2.Distance(transform.position, _currentItem.transform.position) > 1.5f)
                {
                    ReleaseItem();
                }
                return;
            }
            
            // 4. 强制位置
            _currentItem.transform.localPosition = new Vector3(0, 0, -0.1f);
            
            // 5. 强制物理状态
            // 确保吸附且未拖拽时为 Kinematic，防止 DraggableSprite 松手后改回 Dynamic 导致下落
            if (_currentItem.Rigidbody2D != null && !_currentItem.Rigidbody2D.isKinematic)
            {
                _currentItem.Rigidbody2D.isKinematic = true;
                _currentItem.Rigidbody2D.velocity = Vector2.zero;
            }
        }
    }
}
