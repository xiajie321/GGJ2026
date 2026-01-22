using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.SODataScript.MouseCursorSystem
{
    /// <summary>
    /// 鼠标指针动画配置资源类，定义序列帧及其播放参数
    /// </summary>
    [CreateAssetMenu(fileName = "NewUISpriteAnimatorSO", menuName = "MouseCursorSystem/UISpriteAnimator"), Serializable]
    public class SpriteAnimator : ScriptableObject
    {
        public List<Sprite> Sprites; // 序列帧列表
        public float FPS = 5;        // 每秒帧数
        public bool Loop = true;     // 是否循环播放
    }

    /// <summary>
    /// 单个状态的视觉配置，可包含静态精灵或序列帧动画
    /// </summary>
    [Serializable]
    public class CursorStateConfig
    {
        public Sprite Sprite;
        public SpriteAnimator Animator;
    }
}