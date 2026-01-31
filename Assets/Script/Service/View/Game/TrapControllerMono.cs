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
    //添加一个枚举
    // 未刷出：此处没有瑕疵生成，未来可能会刷新。
    // 未遮挡：没被盖住的瑕疵。红边框。洞会播放动画。
    // 已遮挡：已经被盖住的瑕疵。灰色半透明剪影。只会有固定的1帧。
    public class TrapControllerMono:MonoBehaviour,IController
    {
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
        [SerializeField]
        public Collider2D Collider2D => _collider2D;
        public Animator Animator=> _animator;
        public TrapData TrapData => _trapData;
        public TrapAdsorberMono TrapAdsorberMono => _trapAdsorberMono;
        
        private void Start()
        {
            _collider2D ??= GetComponent<Collider2D>();
            _animator ??= GetComponent<Animator>();
            _trapAdsorberMono ??= GetComponent<TrapAdsorberMono>();
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