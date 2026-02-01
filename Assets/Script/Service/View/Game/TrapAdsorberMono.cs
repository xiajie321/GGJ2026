using System;
using DG.Tweening;
using QFramework;
using UnityEngine;

namespace Script.Service.View.Game
{
    public class TrapAdsorberMono : MonoBehaviour
    {
        private bool _isJudgment;
        private ItemControllerMono _currentItem; // 当前已吸附的物品
        private ItemControllerMono _candidateItem; // 当前在范围内但未吸附的物品
        private TrapControllerMono _trapController;
        private Collider2D _collider;

        public bool IsJudgment => _isJudgment; // 有吸附对象
        public ItemControllerMono ItemControllerMono => _currentItem;
        public TrapControllerMono TrapControllerMono => _trapController;

        private void Start()
        {
            _trapController = GetComponent<TrapControllerMono>();
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Equals("Trigger")) return;

            var item = other.GetComponent<ItemControllerMono>();
            if (item == null) return;
            if (item.ItemData.Height != _trapController.TrapData.Height) return;

            // 如果当前没有吸附物品，则将进入的物品设为候选
            if (_currentItem == null)
            {
                _candidateItem = item;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag.Equals("Trigger")) return;

            var item = other.GetComponent<ItemControllerMono>();
            if (item == null) return;

            // 如果离开的是候选物品，清空候选
            if (_candidateItem == item)
            {
                _candidateItem = null;
            }

            // 如果离开的是已吸附物品（通常发生在拖拽离开范围时）
            if (_currentItem == item)
            {
                ReleaseItem();
            }
        }

        private void AdsorbItem(ItemControllerMono item)
        {
            if (_currentItem != null) return; // 已经有物品了

            _isJudgment = true;
            _currentItem = item;
            _currentItem.Parent(transform);

            if (_currentItem.Rigidbody2D != null)
            {
                _currentItem.Rigidbody2D.isKinematic = true;
                _currentItem.Rigidbody2D.velocity = Vector2.zero;
            }

            // 平滑吸附动画
            _currentItem.transform.DOKill();
            _currentItem.transform.DOLocalMove(new Vector3(0, 0, -0.1f), 0.2f).SetEase(Ease.OutQuad);
        }

        private void ReleaseItem()
        {
            _isJudgment = false;
            if (_currentItem != null)
            {
                _currentItem.transform.DOKill();
                
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

            // 2. 吸附逻辑：如果有候选物品且未在拖拽，则吸附
            if (_currentItem == null && _candidateItem != null)
            {
                bool isDragging = _candidateItem.DraggableSprite != null && _candidateItem.DraggableSprite.IsDragging;
                if (!isDragging)
                {
                    AdsorbItem(_candidateItem);
                    _candidateItem = null; // 吸附后不再是候选
                }
            }

            // 3. 释放逻辑：如果已吸附物品开始被拖拽
            if (_currentItem != null)
            {
                bool isDragging = _currentItem.DraggableSprite != null && _currentItem.DraggableSprite.IsDragging;
                if (isDragging)
                {
                    // 变为候选，以便松手后能再次吸附（如果还在范围内）
                    _candidateItem = _currentItem;
                    ReleaseItem();
                    return;
                }
                
                // 4. 检查父物体是否不再是当前陷阱（被其他陷阱吸附）
                if (_currentItem.transform.parent != transform)
                {
                    _isJudgment = false;
                    _currentItem = null;
                    return;
                }
                
                // 5. 强制物理状态（防止意外掉落）
                if (_currentItem.Rigidbody2D != null && !_currentItem.Rigidbody2D.isKinematic)
                {
                    _currentItem.Rigidbody2D.isKinematic = true;
                    _currentItem.Rigidbody2D.velocity = Vector2.zero;
                }
            }
        }
    }
}
