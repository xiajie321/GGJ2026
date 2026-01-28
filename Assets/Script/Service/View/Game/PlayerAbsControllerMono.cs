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
    public class PlayerAbsControllerMono : AbsControllerMono
    {
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
            InitComponents();
            ModeSwitch();//尽量让这个方法在Start中的位置靠后,以免出现赋值为空的情况
        }
        private void Update()
        {
            _fsm.Update();
        }
        private void FixedUpdate()
        {
            _fsm.FixedUpdate();
        }
        private void OnGUI()
        {
            _fsm.OnGUI();
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
    }
}