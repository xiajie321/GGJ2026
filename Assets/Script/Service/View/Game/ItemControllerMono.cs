using QFramework;
using Script.Service.Architecture;
using Script.Service.Command;
using Script.Service.System;
using Script.Service.Utility;
using Script.Service.View.Component;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.View.Game
{
    public class ItemControllerMono:MonoBehaviour,IController
    {
        /// <summary>
        /// 2D 刚体组件
        /// </summary>
        protected Rigidbody2D _rigidbody2D;

        /// <summary>
        /// 2D 碰撞体组件
        /// </summary>
        protected Collider2D _collider2D;
        /// <summary>
        /// 精灵渲染器
        /// </summary>
        protected SpriteRenderer _spriteRenderer;
        protected ItemData  _itemData;
        protected DraggableSprite  _draggableSprite;
        [SerializeField] 
        private int ItemId;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Collider2D Collider2D => _collider2D;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public ItemData ItemData => _itemData;
        public DraggableSprite DraggableSprite => _draggableSprite;
        private void Start()
        {
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
            _collider2D ??= GetComponent<Collider2D>();
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
            _draggableSprite ??= GetComponent<DraggableSprite>();
            InitObject(ItemId);
        }

        private void OnEnable()
        {
            this.SendCommand(new AddItemControllerMonoCommand(this));
        }
        
        public void InitObject(int id)
        {
            if(id == 0 ) return;
            
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
            _collider2D ??= GetComponent<Collider2D>();
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
            _draggableSprite ??= GetComponent<DraggableSprite>();
            _itemData = this.GetUtility<ConfigUtility>().Config.TbItemConfig.Get(id);
            _spriteRenderer.sprite = _itemData.Sprite;
            _draggableSprite.enabled = _itemData.IsMoveable;
            _rigidbody2D.gravityScale = _itemData.Gravity;
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }

        private void OnDisable()
        {
            this.SendCommand(new RemoveItemControllerMonoCommand(this));
            this.GetSystem<FactorySystem>().ItemFactory.Release(gameObject);
        }
    }
}