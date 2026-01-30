using QFramework;
using Script.Service.View.Game.Factory;
using NotImplementedException = System.NotImplementedException;

namespace Script.Service.System
{
    public class FactorySystem:AbstractSystem
    {
        private EnemyFactory _enemyFactory;
        public EnemyFactory EnemyFactory => _enemyFactory;
        private ItemFactory _itemFactory;
        public ItemFactory ItemFactory => _itemFactory;
        private TrapFactory _trapFactory;
        public TrapFactory TrapFactory => _trapFactory;
        protected override void OnInit()
        {
            _enemyFactory = new EnemyFactory();
            EnemyFactory.Init();
            _itemFactory = new ItemFactory();
            ItemFactory.Init();
            _trapFactory = new TrapFactory();
            TrapFactory.Init();
        }

        public void SceneTransition()
        {
            _enemyFactory.SceneChange();
            _itemFactory.SceneChange();
            TrapFactory.SceneChange();
        }
    }
}