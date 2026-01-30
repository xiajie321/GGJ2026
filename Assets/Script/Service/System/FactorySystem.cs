using QFramework;
using Script.Service.View.Game.Factory;
using NotImplementedException = System.NotImplementedException;

namespace Script.Service.System
{
    public class FactorySystem:AbstractSystem
    {
        private EnemyFactory _enemyFactory;
        public EnemyFactory EnemyFactory => _enemyFactory;
        protected override void OnInit()
        {
            _enemyFactory = new EnemyFactory();
            EnemyFactory.Init();
        }

        public void SceneTransition()
        {
            _enemyFactory.SceneChange();
        }
    }
}