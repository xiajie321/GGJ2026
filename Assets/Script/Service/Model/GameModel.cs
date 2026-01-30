using System.Collections.Generic;
using QFramework;
using Script.Service.View;
using Script.Service.View.Game;
using Script.Service.View.Game.Controller;

namespace Script.Service.Model
{
    public class GameModel:AbstractModel
    {
        public List<EnemyControllerMono> EnemyControllerMonos = new();
        public List<ItemControllerMono> ItemControllers = new();
        public List<TrapControllerMono> TrapControllers = new();
        public EnemyGeneratorMono EnemyGeneratorMono;
        protected override void OnInit()
        {
        }
    }
}