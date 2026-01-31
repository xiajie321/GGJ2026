using System;
using QFramework;
using Script.Service.Command;
using Script.Service.Model;
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
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        
        public EnemyDefineController Controller => AbsControllerBase as EnemyDefineController;
        
        public Sprite HappySprite;
        public Sprite ThinkingSprite;
        public Sprite AngrySprite;

        private void Start()
        {
            InitComponents();
            _enemyTriggerMono ??= transform.GetChild(0).GetComponent<EnemyTriggerMono>();
            _spriteRenderer ??= transform.Find("Emoji").GetComponent<SpriteRenderer>();
        }

        public void InitObject(int id)
        {
            InitComponents();
            _enemyTriggerMono ??= transform.GetChild(0).GetComponent<EnemyTriggerMono>();
            _spriteRenderer ??= transform.Find("Emoji").GetComponent<SpriteRenderer>();
            _enemyData = this.GetUtility<ConfigUtility>().Config.TbEnemyConfig.Get(id);
            _animation.runtimeAnimatorController = _enemyData.RuntimeAnimatorController;
            
            this.GetModel<LevelModel>().AddMoney(_enemyData.TicketCost, MoneySource.LevelNpc);
            Debug.Log($"[敌人初始化] 初始化敌人 ID: {id}, 名称: {_enemyData.Name}, 入场费: {_enemyData.TicketCost}, 交互概率: {_enemyData.InteractPBTY}");
            
            SetController(new EnemyDefineController());//TODO 默认控制器
        }

        public override void OnSetData()
        {
            EnemyDefineController ls =((EnemyDefineController)AbsControllerBase);
            ls.SetEnemyData(_enemyData);
            ls.SetEnemyTriggerMono(_enemyTriggerMono);
            ls.SetEmojiRenderer(_spriteRenderer);
            ls.SetEmojiSprites(HappySprite, ThinkingSprite, AngrySprite);
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
