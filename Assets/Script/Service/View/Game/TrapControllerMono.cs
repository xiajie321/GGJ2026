using QFramework;
using Script.Service.Architecture;
using Script.Service.Command;
using Script.Service.System;
using Script.Service.Utility;
using Script.Service.View.Game;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.View
{
    /// <summary>
    /// 瑕疵（陷阱）状态
    /// </summary>
    public enum TrapState
    {
        /// <summary>
        /// 未刷出：此处没有瑕疵生成，未来可能会刷新。
        /// </summary>
        NotSpawned,
        /// <summary>
        /// 未遮挡：没被盖住的瑕疵。红边框。洞会播放动画。
        /// </summary>
        NotBlocked,
        /// <summary>
        /// 已遮挡：已经被盖住的瑕疵。灰色半透明剪影。只会有固定的1帧。
        /// </summary>
        Blocked
    }

    public class TrapControllerMono:MonoBehaviour,IController
    {
        [SerializeField]
        private TrapState _state = TrapState.NotSpawned;
        
        /// <summary>
        /// 当前瑕疵状态
        /// </summary>
        public TrapState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                OnStateChanged();
            }
        }

        private void OnStateChanged()
        {
            switch (_state)
            {
                case TrapState.NotSpawned:
                    _animator.Play("");
                    _spriteRenderer.enabled = false;
                    _collider2D.enabled = false;
                    break;
                case TrapState.NotBlocked:
                    _spriteRenderer.enabled = true;
                    _animator.Play("");
                    _collider2D.enabled = true;
                    break;
                case TrapState.Blocked:
                    _animator.Play("");
                    break;
            }
        }
        /// <summary>
        /// 2D 碰撞体组件
        /// </summary>
        protected Collider2D _collider2D;
        /// <summary>
        /// 精灵渲染器
        /// </summary>
        protected Animator _animator;
        protected TrapData _trapData;
        protected TrapAdsorberMono _trapAdsorberMono;
        protected SpriteRenderer _spriteRenderer;
        [SerializeField]
        public Collider2D Collider2D => _collider2D;
        public Animator Animator=> _animator;
        public TrapData TrapData => _trapData;
        public TrapAdsorberMono TrapAdsorberMono => _trapAdsorberMono;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        
        private void Start()
        {
            _collider2D ??= GetComponent<Collider2D>();
            _animator ??= GetComponent<Animator>();
            _trapAdsorberMono ??= GetComponent<TrapAdsorberMono>();
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
            OnStateChanged();
        }
        private void OnEnable()
        {
            this.SendCommand(new AddTrapControllerMonoCommand(this));
        }

        public void InitObject(int id)
        {
            _collider2D ??= GetComponent<Collider2D>();
            _animator ??= GetComponent<Animator>();
            _trapAdsorberMono ??= GetComponent<TrapAdsorberMono>();
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
            _trapData = this.GetUtility<ConfigUtility>().Config.TbTrapConfig.Get(id);
            _animator.runtimeAnimatorController = _trapData.RuntimeAnimatorController;
        }
        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
        private void OnDisable()
        {
            this.SendCommand(new RemoveTrapControllerMonoCommand(this));
            this.GetSystem<FactorySystem>().TrapFactory.Release(gameObject);
        }
    }
}