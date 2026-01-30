using QFramework;
using Script.Service.Model;
using Script.Service.System;
using Script.Service.View.Game.Controller;

namespace Script.Service.Command
{
    public class RemoveEnemyControllerMonoCommand : AbstractCommand
    {
        private GameModel _model;
        EnemyControllerMono _enemyControllerMono;

        public RemoveEnemyControllerMonoCommand(EnemyControllerMono enemyControllerMono)
        {
            _enemyControllerMono = enemyControllerMono;
        }

        protected override void OnExecute()
        {
            _model = this.GetModel<GameModel>();
            if (!_model.EnemyControllerMonos.Contains(_enemyControllerMono)) return;
            _model.EnemyControllerMonos.Remove(_enemyControllerMono);
            
        }
    }
}