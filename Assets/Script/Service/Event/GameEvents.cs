using UnityEditor.SearchService;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Script.Service.Event
{
    /// <summary>
    /// 游戏进入事件(游戏的开始事件)进入正式游戏场景时会使用
    /// </summary>
    public struct GameEnterEvent
    {
    }
    /// <summary>
    /// 游戏退出事件(退出游戏内的事件)退出游戏玩法场景时会使用
    /// </summary>
    public struct GameExitEvent
    {
    }
    /// <summary>
    /// 游戏运行事件(用于游戏由暂停切换回运行时)
    /// </summary>
    public struct GamePlayEvent
    {
    }
    /// <summary>
    /// 游戏暂停事件(暂停游戏)
    /// </summary>
    public struct GamePauseEvent
    {
    }
    /// <summary>
    /// 游戏重置事件(重置当前关卡的事件)
    /// </summary>
    public struct GameResumeEvent
    {
    }
    /// <summary>
    /// 游戏内关卡切换事件(在游戏内切换关卡的事件)
    /// </summary>
    public struct GameLevelTransitionEvent
    {
    }
    /// <summary>
    /// 场景切换事件
    /// </summary>
    public struct SceneChangeEvent
    {
        public Scene CurrentScene;
        public Scene TargetScene;
    }
}