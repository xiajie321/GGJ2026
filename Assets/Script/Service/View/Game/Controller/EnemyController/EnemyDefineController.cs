using QFramework;
using Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.View.Game.Controller.EnemyController
{
    public class EnemyDefineControllerData
    {
        public Animator Animator;
        public Rigidbody2D Rigidbody2D;
        public Collider2D Collider2D;
        public EnemyTriggerMono EnemyTriggerMono;
        public EnemyData EnemyData;
        
        public int CurrentMood;
        public int TotalLostMood;
        public SpriteRenderer EmojiRenderer;
        public bool IsAngry;
        public Sprite HappySprite;
        public Sprite ThinkingSprite;
        public Sprite AngrySprite;
    }
    public class EnemyDefineController:AbsControllerBase<EnemyState>
    {
        private EnemyData _enemyData;
        private EnemyTriggerMono _enemyTriggerMono;
        private SpriteRenderer _emojiRenderer;
        private Sprite _happySprite;
        private Sprite _thinkingSprite;
        private Sprite _angrySprite;
        
        public EnemyDefineControllerData Data { get; private set; }
        
        protected override void Init(FSM<EnemyState> fsm)
        {
            Data = new EnemyDefineControllerData()
            {
                Animator = _animation,
                Rigidbody2D = _rigidbody2D,
                Collider2D = _collider2D,
                EnemyTriggerMono = _enemyTriggerMono,
                EnemyData = _enemyData,
                
                CurrentMood = _enemyData.MaxMood,
                TotalLostMood = 0,
                EmojiRenderer = _emojiRenderer,
                IsAngry = false,
                HappySprite = _happySprite,
                ThinkingSprite = _thinkingSprite,
                AngrySprite = _angrySprite
            };
            UpdateEmoji(Data);
            
            fsm.AddState(EnemyState.Idle,new EnemyDefineIdleState(fsm,Data));
            fsm.AddState(EnemyState.Move,new EnemyDefineMoveState(fsm,Data));
            fsm.AddState(EnemyState.Interaction,new EnemyDefineInteractionState(fsm,Data));
            fsm.StartState(EnemyState.Idle);
        }

        public void SetEnemyData(EnemyData data)
        {
            _enemyData = data;
        }

        public void SetEnemyTriggerMono(EnemyTriggerMono triggerMono)
        {
            _enemyTriggerMono =  triggerMono;
        }
        
        public void SetEmojiRenderer(SpriteRenderer renderer)
        {
            _emojiRenderer = renderer;
        }
        
        public void SetEmojiSprites(Sprite happy, Sprite thinking, Sprite angry)
        {
            _happySprite = happy;
            _thinkingSprite = thinking;
            _angrySprite = angry;
        }
        
        public static void UpdateEmoji(EnemyDefineControllerData data)
        {
            if (data.EmojiRenderer == null) return;
            
            if (data.TotalLostMood < data.EnemyData.MoodHp1)
            {
                data.EmojiRenderer.sprite = data.HappySprite;
                Debug.Log($"[敌人表情] 设置为开心。累计损失: {data.TotalLostMood}, 阈值1: {data.EnemyData.MoodHp1}");
            }
            else if (data.TotalLostMood < data.EnemyData.MoodHp2)
            {
                data.EmojiRenderer.sprite = data.ThinkingSprite;
                Debug.Log($"[敌人表情] 设置为思考。累计损失: {data.TotalLostMood}, 阈值2: {data.EnemyData.MoodHp2}");
            }
            else
            {
                data.EmojiRenderer.sprite = data.AngrySprite;
                Debug.Log($"[敌人表情] 设置为生气。累计损失: {data.TotalLostMood}");
            }
        }
    }
}
