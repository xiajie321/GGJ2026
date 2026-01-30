using QFramework;
using Script.Service.Model;

namespace Script.Service.Command
{
    public class ClearTrapControllerMonoCommand:AbstractCommand
    {
        private GameModel _model;
        protected override void OnExecute()
        {
            _model = this.GetModel<GameModel>();
            _model.TrapControllers.Clear();
        }
    }
}