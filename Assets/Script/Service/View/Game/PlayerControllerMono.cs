using System;
using Script.Service.View.Game.Controller;
using QFramework;
using Script.Service.View.Game.Controller.PlayerController;
using UnityEngine;

namespace Script.Service.View.Game
{
    public enum PlayerControllerMode
    {
        SideView,//侧面观察模式
        TopDown//俯视角模式
    }
    public class PlayerControllerMono : MonoBehaviour
    {
        private Animator _animation;//动画控制
        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;
        private Controller.Controller _controller;
        private FSM<State> _fsm = new();
        [SerializeField]
        private PlayerControllerMode _mode;
        public PlayerControllerMode Mode
        {
            get => _mode;
            set
            {
                if(_mode == value) return;
                _mode = value;
                ModeSwitch();
            }
        }
        public Animator Animation => _animation;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Collider2D Collider2D => _collider2D;
        private void Start()
        {
            _animation ??= GetComponent<Animator>();
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
            _collider2D ??= GetComponent<Collider2D>();
            ModeSwitch();//尽量让这个方法在Start中的位置靠后,以免出现赋值为空的情况
        }

        private void ModeSwitch()//TODO 控制器切换
        {
            switch (_mode)
            {
                case PlayerControllerMode.SideView:
                    SetController(new PlayerSideViewController());
                    break;
                case PlayerControllerMode.TopDown:
                    SetController(new PlayerTopDownController());
                    break;
                default:
                    SetController(new PlayerSideViewController());
                    break;
            }
        }

        private void Update()
        {
            _fsm.Update();
        }

        private void FixedUpdate()
        {
            _fsm.FixedUpdate();
        }

        public void SetController<T>(T controller)where T : Controller.Controller, new()//TODO 设置对应的控制器
        {
            _controller = controller;
            _controller.SetAnimation(_animation);
            _controller.SetRigidbody2D(_rigidbody2D);
            _controller.SetFsm(_fsm);
            _controller.SetCollider2D(_collider2D);
        }
    }
}