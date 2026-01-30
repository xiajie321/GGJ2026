using UnityEngine;

namespace Script.Service.Interface
{
    /// <summary>
    /// 可拖拽接口
    /// </summary>
    public interface ICanDrag
    {
        /// <summary>
        /// 是否可以在 X 轴拖拽
        /// </summary>
        bool CanDragX { get; set; }
        
        /// <summary>
        /// 是否可以在 Y 轴拖拽
        /// </summary>
        bool CanDragY { get; set; }
        
        /// <summary>
        /// 拖拽开始回调
        /// </summary>
        void OnDragStart(Vector2 position);
        
        /// <summary>
        /// 拖拽中回调
        /// </summary>
        void OnDragging(Vector2 position);
        
        /// <summary>
        /// 拖拽结束回调
        /// </summary>
        void OnDragEnd(Vector2 position);
    }
}
