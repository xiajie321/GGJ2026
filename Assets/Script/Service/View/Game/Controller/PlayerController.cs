using QFramework;
using Script.Service.Utility;
using Script.Service.View.Game.FSMState.PlayerControllerState;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class PlayerController:AbsControllerBase
    {
        public override int Hp { get; set; }

        public override float Speed { get; set; }
        public override int Attack { get; set; }

        public override void Harm(HarmData data)
        {
            if (mFSM.CurrentStateId == PlayerStateEnum.Invincible) return;

            Hp -= data.Hp;
            if (Hp > 0)
            {
                mFSM.ChangeState(PlayerStateEnum.Invincible);
            }
        }
        private int _speed;
        private int _hp;
        private Rigidbody2D _rigidbody2D;
        public Rigidbody2D Rigidbody => _rigidbody2D;

        [SerializeField]
        private GameObject attackGameObject;//用于鼠标控制attack的游戏对象的开关。
        public GameObject AttackGameObject => attackGameObject;

        GameConfigUility _gameConfig;
        public GameConfigUility GameConfig => _gameConfig;

        private FSM<PlayerStateEnum> mFSM = new FSM<PlayerStateEnum>();

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            _gameConfig = this.GetUtility<GameConfigUility>();
            Animator.runtimeAnimatorController = _gameConfig.GameConfig.PlayerConfig.AnimatorController;
            // 初始化数据
            Hp = _gameConfig.GameConfig.PlayerConfig.MaxHp;
            Speed = _gameConfig.GameConfig.PlayerConfig.MaxSpeed;

            // 初始化状态机
            mFSM.AddState(PlayerStateEnum.Idle, new IdleState(mFSM, this));
            mFSM.AddState(PlayerStateEnum.Move, new MoveState(mFSM, this));
            mFSM.AddState(PlayerStateEnum.Jump, new JumpState(mFSM, this));
            mFSM.AddState(PlayerStateEnum.Attack, new AttackState(mFSM, this));
            mFSM.AddState(PlayerStateEnum.Invincible, new InvincibleState(mFSM, this));
            
            mFSM.StartState(PlayerStateEnum.Idle);
        }

        public void Update()
        {
            mFSM.Update();
        }
    }
}
