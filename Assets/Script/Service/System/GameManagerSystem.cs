using QFramework;
using Script.Service.Event;
using NotImplementedException = System.NotImplementedException;

namespace Script.Service.System
{
    public class GameManagerSystem : AbstractSystem
    {
        protected override void OnInit()
        {
            this.RegisterEvent<GameEnterEvent>(GameEnter);
            this.RegisterEvent<GamePlayEvent>(GamePlay);
            this.RegisterEvent<GamePauseEvent>(GamePause);
            this.RegisterEvent<GameResumeEvent>(GameResume);
            this.RegisterEvent<GameLevelTransitionEvent>(GameLevelTransition);
            this.RegisterEvent<GameExitEvent>(GameExit);
        }

        private void GameEnter(GameEnterEvent gameEnterEvent)//进入游戏关卡触发
        {
        }

        private void GamePlay(GamePlayEvent gamePlayEvent)//开始游戏触发
        {
        }

        private void GamePause(GamePauseEvent gamePauseEvent)//暂停游戏触发
        {
        }

        private void GameResume(GameResumeEvent gameResumeEvent)//重置游戏触发
        {
        }

        private void GameLevelTransition(GameLevelTransitionEvent gameLevelTransitionEvent)//游戏内关卡切换时触发
        {
        }

        private void GameExit(GameExitEvent gameExitEvent)
        {
            
        }
    }
}