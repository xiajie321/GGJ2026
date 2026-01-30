using System.Collections.Generic;
using QFramework;
using Script.Service.View;
using Script.Service.View.Game;

namespace Script.Service.Model
{
    public class GameModel:AbstractModel
    {
        public List<ItemControllerMono> ItemControllers = new();
        public List<TrapControllerMono> TrapControllers = new();
        protected override void OnInit()
        {
        }
    }
}