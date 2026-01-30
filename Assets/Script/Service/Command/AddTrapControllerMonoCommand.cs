using QFramework;
using Script.Service.Model;
using Script.Service.View;
using UnityEngine;

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
            _model.TrapControllers.Sort((current,target) =>
            {
                return current.transform.position.x < target.transform.position.x ? -1 : 1;
            });
        }
    }
}