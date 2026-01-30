using QFramework;
using Script.Service.Model;
using Script.Service.View;
using NotImplementedException = System.NotImplementedException;

namespace Script.Service.Command
{
    public class AddTrapControllerMonoCommand:AbstractCommand
    {
        private GameModel _model;
        TrapControllerMono _trapControllerMono;

        public AddTrapControllerMonoCommand(TrapControllerMono trapControllerMono)
        {
            _trapControllerMono = trapControllerMono;
        }
        protected override void OnExecute()
        {
            _model = this.GetModel<GameModel>();
            _model.TrapControllers.Add(_trapControllerMono);
        }
    }
}