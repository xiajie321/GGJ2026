using System;
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
        
        // 碰撞体，用于获取尺寸
        private Collider2D _collider;
        public Collider2D Collider => _collider;

        [SerializeField]
        private GameObject attackGameObject;//用于鼠标控制attack的游戏对象的开关。
        public GameObject AttackGameObject => attackGameObject;
        
        [SerializeField]
        private float groundCheckDistance = 0.1f; // 向下射线检测距离
        
        // 墙壁检测距离
        [SerializeField]
        private float wallCheckDistance = 0.1f;
        
        private LayerMask groundLayer; // 地面图层

        public bool IsGrounded { get; private set; }
        public bool IsTouchingLeftWall { get; private set; }
        public bool IsTouchingRightWall { get; private set; }

        GameConfigUility _gameConfig;
        public GameConfigUility GameConfig => _gameConfig;

        private FSM<PlayerStateEnum> mFSM = new FSM<PlayerStateEnum>();

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            Animator = GetComponent<Animator>();
            _gameConfig = this.GetUtility<GameConfigUility>();
            if (Animator != null)
                Animator.runtimeAnimatorController = _gameConfig.GameConfig.PlayerConfig.AnimatorController;
            
            // 初始化数据
            Hp = _gameConfig.GameConfig.PlayerConfig.MaxHp;
            Speed = _gameConfig.GameConfig.PlayerConfig.MaxSpeed;
            
            // 获取 Ground 图层
            groundLayer = LayerMask.GetMask("Ground");

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
            // 进行物理检测
            CheckPhysics();
            mFSM.Update();
        }
        
        private void CheckPhysics()
        {
            if (_collider == null) return;
            
            Bounds bounds = _collider.bounds;
            
            // 地面检测：使用射线向下检测
            // 从脚底中心稍微向上一点的位置发射射线，防止穿模时检测失败
            Vector2 rayOrigin = new Vector2(bounds.center.x, bounds.min.y + 0.05f);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance + 0.05f, groundLayer);
            IsGrounded = hit.collider != null;
            
            // 墙壁检测
            // 左侧墙壁
            Vector2 leftRayOrigin = new Vector2(bounds.min.x + 0.05f, bounds.center.y);
            RaycastHit2D leftHit = Physics2D.Raycast(leftRayOrigin, Vector2.left, wallCheckDistance + 0.05f, groundLayer);
            IsTouchingLeftWall = leftHit.collider != null;

            // 右侧墙壁
            Vector2 rightRayOrigin = new Vector2(bounds.max.x - 0.05f, bounds.center.y);
            RaycastHit2D rightHit = Physics2D.Raycast(rightRayOrigin, Vector2.right, wallCheckDistance + 0.05f, groundLayer);
            IsTouchingRightWall = rightHit.collider != null;
            
            // 调试绘制
            Debug.DrawRay(rayOrigin, Vector2.down * (groundCheckDistance + 0.05f), IsGrounded ? Color.green : Color.red);
            Debug.DrawRay(leftRayOrigin, Vector2.left * (wallCheckDistance + 0.05f), IsTouchingLeftWall ? Color.green : Color.red);
            Debug.DrawRay(rightRayOrigin, Vector2.right * (wallCheckDistance + 0.05f), IsTouchingRightWall ? Color.green : Color.red);
        }
    }
}
