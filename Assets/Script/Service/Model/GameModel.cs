using QFramework;
using Script.Service.View.Game.Controller;
using System.Collections.Generic;

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
        /// <summary>
        /// 存活的敌人列表
        /// </summary>
        public List<EnemyController> AliveEnemies = new List<EnemyController>();
        protected override void OnInit()
        {
        }
        
    }
}