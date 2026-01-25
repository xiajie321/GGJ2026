using QFramework;
using Script.Service.Model;
using Script.Service.Utility;
using Script.Service.View.Game.Component;
using System;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class EnemyController:AbsControllerBase
    {
        public override int Hp { get; set; }

        public override float Speed { get; set; }
        public override int Attack { get; set; }

        private GameModel _gameModel;
        private Transform _playerTransform;
        private Rigidbody2D _rigidbody2D;
        private GameConfigUility _gameConfig;
        private float _currentHp;

        public override void Harm(HarmData data)
        {
            _currentHp -= data.Hp;

            if (_currentHp <= 0)
            {
                Die();
            }
        }

        private void Start()
        {
            Animator = GetComponent<Animator>();
            _gameConfig = this.GetUtility<GameConfigUility>();
            _gameModel = this.GetModel<GameModel>();

            if (_gameConfig != null && _gameConfig.GameConfig != null && _gameConfig.GameConfig.EnemyConfig != null)
            {
                var enemyData = _gameConfig.GameConfig.EnemyConfig.Get(1);
                Hp = enemyData.Hp;
                Speed = enemyData.Speed;
                _currentHp = Hp;

                if (enemyData.AnimatorController != null && Animator != null)
                {
                    Animator.runtimeAnimatorController = enemyData.AnimatorController;
                }
            }

            _rigidbody2D = GetComponent<Rigidbody2D>();
            if (_rigidbody2D == null)
            {
                _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
                _rigidbody2D.gravityScale = 0;
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation; // 锁定旋转
            }

            var collider = GetComponent<Collider2D>();
            if (collider == null)
            {
                gameObject.AddComponent<CircleCollider2D>();
            }

            gameObject.tag = "Enemy";
        }

        private void Update()
        {
            if (_playerTransform == null)
            {
                FindPlayer();
                return;
            }

            MoveTowardsPlayer();
            UpdateAnimation();
        }

        private void FindPlayer()
        {
            if (_gameModel != null && _gameModel.PlayerController != null)
            {
                _playerTransform = _gameModel.PlayerController.transform;
            }
            else
            {
                // 备用：标签查找
                var playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    _playerTransform = playerObject.transform;
                }
            }
        }

        private void MoveTowardsPlayer()
        {
            if (_playerTransform == null || _rigidbody2D == null) return;

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
            if (Animator == null) return;

            if (_rigidbody2D != null)
            {
                float speed = _rigidbody2D.velocity.magnitude;
                Animator.SetFloat("Speed", speed);

                bool isMoving = speed > 0.1f;
                Animator.SetBool("IsMoving", isMoving);
            }
        }

        private void Die()
        {
            if (_gameModel != null)
            {
                _gameModel.Points.Value += 30;
            }

            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    var harmData = new HarmData
                    {
                        Hp = Attack
                    };
                    playerController.Harm(harmData);
                }
            }
        }

        private void OnDestroy()
        {
            if (_rigidbody2D != null)
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }
    }
}