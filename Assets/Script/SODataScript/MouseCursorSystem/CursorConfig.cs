using UnityEngine;

namespace Script.SODataScript.MouseCursorSystem
{
    /// <summary>
    /// 鼠标指针完整的状态机配置资源类
    /// </summary>
    [CreateAssetMenu(fileName = "NewCursorConfig", menuName = "MouseCursorSystem/UICursorConfig")]
    public class CursorConfig : ScriptableObject
    {
        public CursorStateConfig DefaultState; // 默认
        public CursorStateConfig HoverState;   // 悬停
        public CursorStateConfig DownState;    // 按下
        public CursorStateConfig UpState;      // 抬起
        public CursorStateConfig MoveState;    // 移动
    }
}