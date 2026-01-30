using QFramework;
using Script.Service.Model;
using Script.Service.View.Game;
using NotImplementedException = System.NotImplementedException;

namespace Script.Service.Command
{
    public class AddItemControllerMonoCommand:AbstractCommand
    {
        private GameModel _model;
        ItemControllerMono _itemControllerMono;

        public AddItemControllerMonoCommand(ItemControllerMono itemControllerMono)
        {
            _itemControllerMono = itemControllerMono;
        }
        protected override void OnExecute()
        {
            _model = this.GetModel<GameModel>();
            _model.ItemControllers.Add(_itemControllerMono);
        }
    }
}