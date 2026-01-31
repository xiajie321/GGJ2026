using System;
using QFramework;
using UnityEngine;

namespace Script.Service.View.Game
{
    public class TrapAdsorberMono:MonoBehaviour
    {
        private bool _isJudgment;
        private ItemControllerMono _gameObject;
        private TrapControllerMono _trapController;
        public bool IsJudgment => _isJudgment;//有媳妇对象
        public ItemControllerMono ItemControllerMono => _gameObject;
        public TrapControllerMono TrapControllerMono => _trapController;

        private void Start()
        {
            _trapController = GetComponent<TrapControllerMono>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag.Equals("Trigger")) return;
            if(_isJudgment) return;
            _gameObject = other.GetComponent<ItemControllerMono>();
            if (_gameObject == null) return;
            if(_gameObject.ItemData.Height != _trapController.TrapData.Height) return;
            _isJudgment = true;
            _gameObject.Parent(transform);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.tag.Equals("Trigger")) return;
            if(!_isJudgment) return;
            if (_gameObject != null && _gameObject.gameObject == other.gameObject)
            {
                if(_gameObject.ItemData.Height != _trapController.TrapData.Height) return;
                _isJudgment = false;
                if (gameObject.activeInHierarchy && _gameObject.gameObject.activeInHierarchy)
                {
                    _gameObject.transform.SetParent(null);
                }
                _gameObject =  null;
            }
        }

        private void Update()
        {
            if(!_isJudgment || _gameObject == null) return;
            
            // 如果物品的父物体不再是当前陷阱，说明它被其他陷阱吸附或被移除了
            if (_gameObject.transform.parent != transform)
            {
                _isJudgment = false;
                _gameObject = null;
                return;
            }

            if(_gameObject.DraggableSprite != null && _gameObject.DraggableSprite.IsDragging) return;
            _gameObject.transform.localPosition = new Vector3(0, 0, -0.1f);
        }
    }
}