using QFramework;
using Script.Service.View.Game.Controller;

namespace Script.Service.Model
{
    public class GameModel:AbstractModel
    {
        /// <summary>
        /// 当前游戏关卡下玩家的控制器
        /// </summary>
        public PlayerController PlayerController;
        /// <summary>
        /// 当前游戏关卡下玩家的积分
        /// </summary>
        public BindableProperty<int> Points =  new();
        protected override void OnInit()
        {
        }
        
    }
}