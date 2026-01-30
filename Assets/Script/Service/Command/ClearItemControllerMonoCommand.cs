using QFramework;
using Script.Service.Model;

namespace Script.Service.Command
{
    public class ClearItemControllerMonoCommand:AbstractCommand
    {
        private GameModel _model;
        protected override void OnExecute()
        {
            _model = this.GetModel<GameModel>();
            _model.ItemControllers.Clear();
        }
    }
}