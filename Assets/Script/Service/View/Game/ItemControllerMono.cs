using QFramework;
using Script.Service.Architecture;
using Script.Service.Command;
using Script.Service.Utility;
using Script.SODataScript.TbConfig;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

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
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Collider2D Collider2D => _collider2D;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public ItemData ItemData => _itemData;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            this.SendCommand(new AddItemControllerMonoCommand(this));
        }

        public void InitObject(int id)
        {
            _itemData = this.GetUtility<ConfigUtility>().Config.TbItemConfig.Get(id);
            _spriteRenderer.sprite = _itemData.Sprite;
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }

        private void OnDisable()
        {
            this.SendCommand(new RemoveItemControllerMonoCommand(this));
        }
    }
}