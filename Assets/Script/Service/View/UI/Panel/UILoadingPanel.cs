using Cysharp.Threading.Tasks;
using QFramework;
using Script.Service.System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Service.View.UI.Panel
{
    public class UILoadingPanelData : UIPanelData
    {
    }
    
    public partial class UILoadingPanel : UIPanel, ISceneSwitch, IController
    {
        [Header("动画组件")]
        [SerializeField] private CanvasGroup mCanvasGroup; // 用于淡入淡出

        [Header("参数设置")]
        [SerializeField] private float fadeSpeed = 2.0f;   // 淡入淡出速度
        [SerializeField] private float minStayTime = 1.0f; // 最小停留时间，防止闪烁

        private float _timer;          // 计时器
        private bool _isFadingOut;     // 标记是否正在执行退出动画

        public IArchitecture GetArchitecture()
        {
            return Script.Service.Architecture.GameArchitecture.Interface;
        }

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UILoadingPanelData ?? new UILoadingPanelData();
            
            // 如果没有在编辑器拖拽 CanvasGroup，尝试自动获取
            if (mCanvasGroup == null) mCanvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            // 初始化状态
            _timer = 0;
            _isFadingOut = false;

            // 1. 设置初始透明度为 0 (完全透明)
            if (mCanvasGroup != null) 
            {
                mCanvasGroup.alpha = 0f;
                mCanvasGroup.blocksRaycasts = true; // 阻挡点击
            }
        }

        /// <summary>
        /// 加载中 (每帧调用)
        /// </summary>
        public void OnLoad(float progress, bool isLoad)
        {
            // 1. 处理淡入动画 (Alpha 从 0 到 1)
            if (mCanvasGroup != null && mCanvasGroup.alpha < 1f && !_isFadingOut)
            {
                mCanvasGroup.alpha = Mathf.MoveTowards(mCanvasGroup.alpha, 1f, Time.deltaTime * fadeSpeed);
            }
            
            Debug.Log($"正在加载... 实际进度: {progress * 100}%");
        }
        
        /// <summary>
        /// 加载完成检测 (每帧调用，直到返回 true)
        /// </summary>
        public bool OnLoadCompleted(float progress, bool isLoad)
        {
            // 计时
            _timer += Time.deltaTime;

            if (_timer < minStayTime) 
            {
                return false;
            }

            // 开始淡出逻辑
            _isFadingOut = true;

            if (mCanvasGroup != null)
            {
                // 执行淡出 (Alpha 从 1 到 0)
                mCanvasGroup.alpha = Mathf.MoveTowards(mCanvasGroup.alpha, 0f, Time.deltaTime * fadeSpeed);

                // 如果完全透明了，说明动画结束，可以关闭面板了
                if (mCanvasGroup.alpha <= 0.01f)
                {
                    mCanvasGroup.blocksRaycasts = false;
                    return true; // 返回 true，系统会销毁或隐藏此面板
                }
                
                // 还没淡出完，继续保持
                return false; 
            }
            
            return true;
        }

        protected override void OnShow() { }
        protected override void OnHide() { }
        protected override void OnClose() { }
    }
}
