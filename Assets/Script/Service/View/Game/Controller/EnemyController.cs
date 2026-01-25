using QFramework;
using Script.Service.Model;
using Script.Service.Utility;
using System;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class EnemyController:AbsControllerBase
    {
        public override int Hp { get; set; }

        public override int Speed { get; set; }


        private GameModel _gameModel;
        private Transform _playerTransform;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        public override void Harm(HarmData data)
        {
            Hp -= data.Damage;

            if (Hp <= 0)
            {
                Die();
            }
        }

        GameConfigUility _gameConfig;
        private void Start()
        {
            _gameModel = this.GetModel<GameModel>();
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            if (_gameModel.PlayerController != null)
            {
                _playerTransform = _gameModel.PlayerController.transform;
            }
        }

        private void Update()
        {
            if (_playerTransform == null || _gameModel.PlayerController == null) return;

            MoveTowardsPlayer();
            UpdateAnimation();
        }

        private void MoveTowardsPlayer()
        {
            if (_rigidbody2D == null) return;

            Vector2 direction = (_playerTransform.position - transform.position).normalized;
            _rigidbody2D.velocity = direction * Speed;

            // 翻转Sprite朝向
            if (direction.x != 0)
            {
                Vector3 localScale = transform.localScale;
                localScale.x = Mathf.Sign(direction.x) * Mathf.Abs(localScale.x);
                transform.localScale = localScale;
            }
        }

        private void UpdateAnimation()
        {
            if (_animator == null) return;

            if (_rigidbody2D != null)
            {
                float speed = _rigidbody2D.velocity.magnitude;
                _animator.SetFloat("Speed", speed);

                bool isMoving = speed > 0.1f;
                _animator.SetBool("IsMoving", isMoving);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // 检测与玩家的碰撞
            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 对玩家造成伤害
                var harmData = new HarmData
                {
                    Damage = 10, // 敌人攻击力
                    Attacker = this
                };
                playerController.Harm(harmData);
            }
        }

        private void Die()
        {
            // 增加分数
            _gameModel.Points.Value += 30;

            // 发送敌人死亡事件
            this.SendEvent(new EnemyDeathEvent
            {
                EnemyController = this,
                Position = transform.position
            });

            // 销毁敌人
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            // 清理工作
            if (_rigidbody2D != null)
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }
    }

    public class EnemyDeathEvent
    {
        public EnemyController EnemyController;
        public Vector2 Position;
    }
}