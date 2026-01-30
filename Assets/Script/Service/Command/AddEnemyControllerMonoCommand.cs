using QFramework;
using Script.Service.Model;
using Script.Service.View.Game;
using Script.Service.View.Game.Controller;

namespace Script.Service.Command
{
    public class AddEnemyControllerMonoCommand:AbstractCommand
    {
        private GameModel _model;
        EnemyControllerMono _enemyControllerMono;

        public AddEnemyControllerMonoCommand(EnemyControllerMono enemyControllerMono)
        {
            _enemyControllerMono = enemyControllerMono;
        }
        protected override void OnExecute()
        {
            _model = this.GetModel<GameModel>();
            _model.EnemyControllerMonos.Add(_enemyControllerMono);
        }
    }
}