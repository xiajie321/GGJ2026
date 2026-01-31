using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Service.View.UI.Component
{
    public class UIElasticScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("基础设置")]
        [Tooltip("基础大小")]
        public float BaseScale = 1f;
        [Tooltip("鼠标悬停/高亮时的大小")]
        public float HoverScale = 1.1f;
        [Tooltip("按下时的大小")]
        public float PressScale = 0.95f;
        [Tooltip("动画时间")]
        public float Duration = 0.3f;

        [Header("动画手感")]
        [Tooltip("使用 DOTween 的 Ease 类型，OutBack 最适合做弹性效果")]
        public Ease AnimationEase = Ease.OutBack;
        [Tooltip("如果需要完全自定义曲线，可以将此项设为 true 并设置下方的 Curve")]
        public bool UseCustomCurve = false;
        public AnimationCurve CustomCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1.2f), new Keyframe(1, 1));

        [Header("闲置循环 (鼠标未进入时播放)")]
        [Tooltip("是否开启待机呼吸动画")]
        public bool EnableIdleLoop = false;
        [Tooltip("呼吸动画的目标大小")]
        public float IdleScale = 1.05f;
        [Tooltip("呼吸一次的时间")]
        public float IdleDuration = 1f;

        private Vector3 _originalScale;
        private Tweener _currentTweener;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void OnEnable()
        {
            ResetScale();
            
            if (EnableIdleLoop)
            {
                PlayIdleLoop();
            }
        }

        private void OnDisable()
        {
            transform.DOKill();
        }
        
        public void PlayScaleTween(float targetMult, bool isLoop = false)
        {
            transform.DOKill();
            
            Vector3 targetScale = _originalScale * targetMult;
            
            if (isLoop)
            {
                _currentTweener = transform.DOScale(targetScale, IdleDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetUpdate(true);
            }
            else
            {
                var tween = transform.DOScale(targetScale, Duration).SetUpdate(true);
                
                if (UseCustomCurve)
                    tween.SetEase(CustomCurve);
                else
                    tween.SetEase(AnimationEase);
                
                _currentTweener = tween;
            }
        }

        /// <summary>
        /// 外部调用接口：手动播放一次“Q弹”效果
        /// 用于新手引导或代码触发
        /// </summary>
        public void TriggerElasticEffect()
        {
            // 先放大，再自动恢复
            transform.DOScale(_originalScale * HoverScale, Duration * 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    PlayScaleTween(BaseScale);
                });
        }

        // --- 内部逻辑 ---

        private void ResetScale()
        {
            transform.localScale = _originalScale * BaseScale;
        }

        private void PlayIdleLoop()
        {
            PlayScaleTween(IdleScale, true);
        }
        

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayScaleTween(HoverScale);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // 恢复原状
            if (EnableIdleLoop)
            {
                transform.DOScale(_originalScale * BaseScale, 0.2f)
                    .SetUpdate(true)
                    .OnComplete(PlayIdleLoop);
            }
            else
            {
                PlayScaleTween(BaseScale);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PlayScaleTween(PressScale);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            bool isHovering = eventData.hovered.Contains(gameObject);
            PlayScaleTween(isHovering ? HoverScale : BaseScale);
            
            if (!isHovering && EnableIdleLoop)
            {
            }
        }
    }
}