using UnityEngine;

namespace Script.SODataScript.MouseCursorSystem
{
    /// <summary>
    /// 鼠标指针完整的状态机配置资源类
    /// </summary>
    [CreateAssetMenu(fileName = "NewCursorConfig", menuName = "MouseCursorSystem/UICursorConfig")]
    public class CursorConfig : ScriptableObject
    {
        public CursorStateConfig DefaultState= new(); // 默认
        public CursorStateConfig HoverState = new();   // 悬停
        public CursorStateConfig DownState = new();    // 按下
        public CursorStateConfig UpState= new();      // 抬起
        public CursorStateConfig MoveState= new();    // 移动

        [Header("参数配置")]
        [Tooltip("判断是否为静止状态的阈值")]
        public float IdleThreshold = 0.1f;
        
        [Tooltip("能够切换到移动状态的阈值")]
        public float MoveThreshold = 1.0f;

        [Header("移动状态动画速度配置")]
        [Tooltip("是否启用根据移动速度决定移动状态的播放速度")]
        public bool EnableSpeedBasedAnim = false;

        [Tooltip("根据移动速度决定移动状态动画播放速度的最小阈值")]
        public float MinSpeedThreshold = 100.0f;

        [Tooltip("根据移动速度决定移动状态动画播放速度的最大阈值")]
        public float MaxSpeedThreshold = 1000.0f;

        [Header("动态 FPS 配置")]
        [Tooltip("移动状态时的最小 FPS (对应最小速度阈值)")]
        public float MinFPS = 5.0f;

        [Tooltip("移动状态时的最大 FPS (对应最大速度阈值)")]
        public float MaxFPS = 30.0f;
    }
}
