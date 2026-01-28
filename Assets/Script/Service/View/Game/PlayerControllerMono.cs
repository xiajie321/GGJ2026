using Alchemy.Inspector;
using Script.Service.View.Game.Controller;
using Script.Service.View.Game.Controller.PlayerController;
using UnityEngine;

namespace Script.Service.View.Game
{
    /// <summary>
    /// 玩家控制模式
    /// </summary>
    public enum PlayerControllerMode
    {
        /// <summary>
        /// 侧视角模式 (Platformer)
        /// </summary>
        SideView,
        /// <summary>
        /// 俯视角模式 (TopDown)
        /// </summary>
        TopDown
    }

    /// <summary>
    /// 玩家控制器的 MonoBehaviour 实现，负责处理模式切换和生命周期回调
    /// </summary>
    public class PlayerControllerMono : AbsControllerBaseMono<PlayerState>
    {
        private PlayerControllerMode _mode = PlayerControllerMode.SideView;

        /// <summary>
        /// 当前玩家控制模式，设置时会自动切换控制器
        /// </summary>
        [ShowInInspector]
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