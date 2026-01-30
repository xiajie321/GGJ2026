using System;
using QFramework;
using UnityEngine;

namespace Script.Service.View.Game
{
    public class TrapAdsorberMono:MonoBehaviour
    {
        private bool _isJudgment;
        private ItemControllerMono _gameObject;
        public bool IsJudgment => _isJudgment;//有媳妇对象
        public ItemControllerMono ItemControllerMono => _gameObject;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag.Equals("Trigger")) return;
            if(_isJudgment) return;
            _gameObject = other.GetComponent<ItemControllerMono>();
            _isJudgment = true;
            _gameObject.Parent(transform);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.tag.Equals("Trigger")) return;
            if(!_isJudgment) return;
            if (_gameObject.gameObject == other.gameObject)
            {
                _isJudgment = false;
                _gameObject.transform.parent = null;
            }
        }

        private void Update()
        {
            if(!_isJudgment) return;
            if(ItemControllerMono.DraggableSprite.IsDragging) return;
            _gameObject.transform.position = transform.position;//TODO 可以设置
        }
    }
}