using System;
using QFramework;
using Script.Service.Command;
using Script.Service.System;
using Script.Service.Utility;
using Script.Service.View.Game.Controller.EnemyController;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    /// <summary>
    /// 敌人控制器的 MonoBehaviour 实现
    /// </summary>
    public class EnemyControllerMono:AbsControllerBaseMono<EnemyState>
    {
        public Animator Animation => _animation;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Collider2D Collider2D => _collider2D;
        private EnemyData _enemyData;
        public EnemyData EnemyData => _enemyData;
        private EnemyTriggerMono _enemyTriggerMono;
        public EnemyTriggerMono EnemyTriggerMono => _enemyTriggerMono;

        private void Start()
        {
            InitComponents();
            _enemyTriggerMono = transform.GetChild(0).GetComponent<EnemyTriggerMono>();
            InitObject(0);
        }

        public void InitObject(int id)
        {
            _enemyData = this.GetUtility<ConfigUtility>().Config.TbEnemyConfig.Get(id);
            _animation.runtimeAnimatorController = _enemyData.RuntimeAnimatorController;
            SetController(new EnemyDefineController());//TODO 默认控制器
        }

        public override void OnSetData()
        {
            EnemyDefineController ls =((EnemyDefineController)AbsControllerBase);
            ls.SetEnemyData(_enemyData);
            ls.SetEnemyTriggerMono(_enemyTriggerMono);
        }

        private void OnEnable()
        {
            this.SendCommand(new AddEnemyControllerMonoCommand(this));
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

        private void OnDisable()
        {
            this.SendCommand(new RemoveEnemyControllerMonoCommand(this));
            this.GetSystem<FactorySystem>().EnemyFactory.Release(gameObject);
        }
    }
}