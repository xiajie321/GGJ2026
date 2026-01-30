using QFramework;
using Script.Service.Model;
using Script.Service.View;

namespace Script.Service.Command
{
    public class RemoveTrapControllerMonoCommand:AbstractCommand
    {
        private GameModel _model;
        TrapControllerMono _trapControllerMono;

        public RemoveTrapControllerMonoCommand(TrapControllerMono trapControllerMono)
        {
            _trapControllerMono = trapControllerMono;
        }
        protected override void OnExecute()
        {
            _model = this.GetModel<GameModel>();
            if(_model.TrapControllers.Contains(_trapControllerMono))
                _model.TrapControllers.Remove(_trapControllerMono);
        }
    }
}