using QFramework;
using Script.Service.Model;
using NotImplementedException = System.NotImplementedException;

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