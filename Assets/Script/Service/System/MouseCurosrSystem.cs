using System;
using QFramework;
using Script.Service.View.UI.Panel;
using Script.SODataScript.MouseCursorSystem;
using UnityEngine;

namespace Script.Service.System
{
    /// <summary>
    /// 鼠标指针系统，负责管理自定义鼠标指针的显示、位置及相关交互事件
    /// </summary>
    public class MouseCursorSystem:AbstractSystem
    {
        /// <summary>
        /// 鼠标指针面板引用
        /// </summary>
        UIMouseCursorPanel _mouseCursorPanel;

        protected override void OnInit()
        {
#if !UNITY_EDITOR
                // 非编辑器模式下隐藏系统默认鼠标
                Cursor.visible = false;
#endif
            if(_mouseCursorPanel) return;
            // 打开自定义鼠标指针面板
            _mouseCursorPanel = UIKit.OpenPanel<UIMouseCursorPanel>(UILevel.PopUI);
        }

        /// <summary>
        /// 设置默认状态的鼠标指针图标
        /// </summary>
        /// <param name="sprite">图标精灵</param>
        public void SetDefaultCursorIcon(Sprite sprite)
        {
            _mouseCursorPanel.SetDefaultStateConfig(sprite);
        }

        /// <summary>
        /// 设置默认状态的鼠标指针动画
        /// </summary>
        /// <param name="spriteAnimator">动画配置</param>
        public void SetDefaultCursorAnimator(SpriteAnimator spriteAnimator)
        {
            _mouseCursorPanel.SetDefaultStateConfig(spriteAnimator);
        }

        /// <summary>
        /// 设置鼠标指针图标 (兼容旧接口，直接设置当前显示，不建议用于状态切换逻辑)
        /// </summary>
        /// <param name="sprite">图标精灵</param>
        public void SetCursorIcon(Sprite sprite)
        {
            _mouseCursorPanel.SetCursorIcon(sprite);
        }

        /// <summary>
        /// 设置鼠标指针配置
        /// </summary>
        /// <param name="config">鼠标指针配置</param>
        public void SetCursorConfig(CursorConfig config)
        {
            _mouseCursorPanel.SetConfig(config);
        }

        /// <summary>
        /// 设置鼠标指针偏移
        /// </summary>
        /// <param name="offset">偏移向量</param>
        public void SetCursorOffset(Vector2 offset)
        {
            _mouseCursorPanel.SetCursorOffset(offset);
        }

        /// <summary>
        /// 获取鼠标指针的变换组件
        /// </summary>
        /// <returns>鼠标指针的 Transform</returns>
        public Transform GetCursorTransform()
        {
            return _mouseCursorPanel.GetCursorTransform();
        }

        /// <summary>
        /// 添加鼠标进入 UI 元素的事件回调
        /// </summary>
        /// <param name="action">回调动作</param>
        public void AddMouseEnterEvent(Action action)
        {
            _mouseCursorPanel.AddMouseEnterEvent(action);
        }

        /// <summary>
        /// 移除鼠标进入 UI 元素的事件回调
        /// </summary>
        /// <param name="action">回调动作</param>
        public void RemoveMouseEnterEvent(Action action)
        {
            _mouseCursorPanel.RemoveMouseEnterEvent(action);
        }

        /// <summary>
        /// 添加鼠标离开 UI 元素的事件回调
        /// </summary>
        /// <param name="action">回调动作</param>
        public void AddMouseExitEvent(Action action)
        {
            _mouseCursorPanel.AddMouseExitEvent(action);
        }

        /// <summary>
        /// 移除鼠标离开 UI 元素的事件回调
        /// </summary>
        /// <param name="action">回调动作</param>
        public void RemoveMouseExitEvent(Action action)
        {
            _mouseCursorPanel.RemoveMouseExitEvent(action);
        }

        /// <summary>
        /// 添加鼠标左键按下的事件回调
        /// </summary>
        /// <param name="action">回调动作</param>
        public void AddMouseDownEvent(Action action)
        {
            _mouseCursorPanel.AddMouseDownEvent(action);
        }

        /// <summary>
        /// 移除鼠标左键按下的事件回调
        /// </summary>
        /// <param name="action">回调动作</param>
        public void RemoveMouseDownEvent(Action action)
        {
            _mouseCursorPanel.RemoveMouseDownEvent(action);
        }

        /// <summary>
        /// 添加鼠标左键抬起的事件回调
        /// </summary>
        /// <param name="action">回调动作</param>
        public void AddMouseUpEvent(Action action)
        {
            _mouseCursorPanel.AddMouseUpEvent(action);
        }

        /// <summary>
        /// 移除鼠标左键抬起的事件回调
        /// </summary>
        /// <param name="action">回调动作</param>
        public void RemoveMouseUpEvent(Action action)
        {
            _mouseCursorPanel.RemoveMouseUpEvent(action);
        }

        /// <summary>
        /// 添加鼠标移动的事件回调
        /// </summary>
        /// <param name="action">回调动作，参数为移动的方向向量</param>
        public void AddMouseMoveEvent(Action<Vector2> action)
        {
            _mouseCursorPanel.AddMouseMoveEvent(action);
        }

        public void ShowMouseCursor(bool show) => _mouseCursorPanel.MouseCursor.gameObject.SetActive(show);

        /// <summary>
        /// 移除鼠标移动的事件回调
        /// </summary>
        /// <param name="action">回调动作</param>
        public void RemoveMouseMoveEvent(Action<Vector2> action)
        {
            _mouseCursorPanel.RemoveMouseMoveEvent(action);
        }
    }
}