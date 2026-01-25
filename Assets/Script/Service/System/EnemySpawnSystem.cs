using QFramework;
using Script.Service.Event;
using Script.Service.Model;
using Script.Service.Utility;
using Script.Service.View.Game.Controller;
using Script.SODataScript.GameConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Service.System
{
    public class EnemySpawnSystem : AbstractSystem
    {
        private GameModel _gameModel;
        private GameConfigUility _gameConfig;
        private Transform _enemyContainer;
        private List<EnemyController> _activeEnemies = new List<EnemyController>();

        // 生成参数
        private float _spawnTimer = 0f;
        private float _spawnInterval = 3f;
        private float _spawnDistance = 8f;
        private int _maxEnemyCount = 5;
        private bool _isSpawning = false;

        protected override void OnInit()
        {
            _gameModel = this.GetModel<GameModel>();
            _gameConfig = this.GetUtility<GameConfigUility>();

            CreateEnemyContainer();

            // 监听游戏状态事件
            this.RegisterEvent<GamePlayEvent>(OnGamePlay);
            this.RegisterEvent<GamePauseEvent>(OnGamePause);
        }

        private void CreateEnemyContainer()
        {
            var containerObj = new GameObject("EnemyContainer");
            _enemyContainer = containerObj.transform;
        }

        private void OnGamePlay(GamePlayEvent e)
        {
            _isSpawning = true;
            _spawnTimer = 0f;
        }

        private void OnGamePause(GamePauseEvent e)
        {
            _isSpawning = false;
        }

        private void Update()
        {
            if (!_isSpawning || _gameModel?.PlayerController == null) return;

            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= _spawnInterval && _activeEnemies.Count < _maxEnemyCount)
            {
                _spawnTimer = 0f;
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            if (_gameModel?.PlayerController == null) return;

            Vector3 playerPosition = _gameModel.PlayerController.transform.position;

            float spawnDirection = Random.Range(0, 2) * 2 - 1;

            Vector3 spawnPosition = playerPosition + new Vector3(
                spawnDirection * _spawnDistance,
                0,
                0
            );

            // 加载预制体
            GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
            if (enemyPrefab == null)
            {
                Debug.LogError("[EnemySpawnSystem] 未找到敌人预制体: Resources/Prefabs/Enemy");
                return;
            }

            GameObject enemyObj = GameObject.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, _enemyContainer);
            EnemyController enemyController = enemyObj.GetComponent<EnemyController>();

            if (enemyController != null)
            {
                SetupEnemy(enemyController);
                _activeEnemies.Add(enemyController);

                Debug.Log($"[EnemySpawnSystem] 在位置 {spawnPosition} 生成敌人");
            }
        }

        private void SetupEnemy(EnemyController enemyController)
        {
            if (_gameConfig == null) return;

            // 获取敌人配置
            TbEnemyConfig enemyConfig = _gameConfig.GetEnemyConfig(10001);
            enemyController.Hp = enemyConfig.Hp;
            enemyController.Speed = enemyConfig.Speed;
            enemyController.Attack = 10;
        }

        public void RemoveEnemy(EnemyController enemyController)
        {
            if (enemyController != null && _activeEnemies.Contains(enemyController))
            {
                _activeEnemies.Remove(enemyController);
            }
        }
    }
}