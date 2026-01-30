using QFramework;
using Script.Service.Model;
using Script.Service.View.Game;

namespace Script.Service.Command
{
    public class RemoveItemControllerMonoCommand:AbstractCommand
    {
        private GameModel _model;
        ItemControllerMono _itemControllerMono;

        public RemoveItemControllerMonoCommand(ItemControllerMono itemControllerMono)
        {
            _itemControllerMono = itemControllerMono;
        }
        protected override void OnExecute()
        {
            _model = this.GetModel<GameModel>();
            if(_model.ItemControllers.Contains(_itemControllerMono))
                _model.ItemControllers.Remove(_itemControllerMono);
        }
    }
}