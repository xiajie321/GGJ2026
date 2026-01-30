using QFramework;
using Script.Service.Model;
using Script.Service.View.Game;

namespace Script.Service.Command
{
    public class SetEnemyGeneratorMonoCommand:AbstractCommand
    {
        private GameModel _model;
        private EnemyGeneratorMono _enemyGeneratorMono;

        public SetEnemyGeneratorMonoCommand(EnemyGeneratorMono enemyGeneratorMono)
        {
            _enemyGeneratorMono = enemyGeneratorMono;
        }
        protected override void OnExecute()
        {
            _model = this.GetModel<GameModel>();
            _model.EnemyGeneratorMono = _enemyGeneratorMono;
        }
    }
}