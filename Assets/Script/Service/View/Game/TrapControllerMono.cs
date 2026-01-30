using QFramework;
using Script.Service.Architecture;
using Script.Service.Command;
using Script.Service.Utility;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.View
{
    public class TrapControllerMono:MonoBehaviour,IController
    {
        /// <summary>
        /// 2D 碰撞体组件
        /// </summary>
        protected Collider2D _collider2D;
        /// <summary>
        /// 精灵渲染器
        /// </summary>
        protected SpriteRenderer _spriteRenderer;
        protected TrapData _trapData;
        public Collider2D Collider2D => _collider2D;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public TrapData TrapData => _trapData;

        private void Start()
        {
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void OnEnable()
        {
            this.SendCommand(new AddTrapControllerMonoCommand(this));
        }

        public void InitObject(int id)
        {
            _trapData = this.GetUtility<ConfigUtility>().Config.TbTrapConfig.Get(id);
            _spriteRenderer.sprite = _trapData.Sprite;
        }
        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
        private void OnDisable()
        {
            this.SendCommand(new RemoveTrapControllerMonoCommand(this));
        }
    }
}