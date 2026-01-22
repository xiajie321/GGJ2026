using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Service.View.UI
{
    public class UIButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        [Header("Settings")] [SerializeField] private bool useScaleEffect = true;
        [SerializeField] private float hoverScale = 1.1f;
        [SerializeField] private float clickScale = 0.95f;
        [SerializeField] private float animDuration = 0.2f;
        [SerializeField] private Ease animEase = Ease.OutBack;

        [Header("Punch Effect (Optional)")] [SerializeField]
        private bool usePunchEffect = false;

        [SerializeField] private Vector3 punchVector = new Vector3(0.1f, 0.1f, 0);

        // 缓存原始缩放值
        private Vector3 _originalScale;
        private RectTransform _rectTransform;
        private Tween _currentTween;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
        }

        private void OnEnable()
        {
            // 每次启用时重置状态，防止物体被隐藏后再显示时状态错误
            _rectTransform.localScale = _originalScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (useScaleEffect)
            {
                KillTween();
                _currentTween = _rectTransform.DOScale(_originalScale * hoverScale, animDuration)
                    .SetEase(animEase)
                    .SetUpdate(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (useScaleEffect)
            {
                KillTween();
                _currentTween = _rectTransform.DOScale(_originalScale, animDuration)
                    .SetEase(Ease.OutQuad)
                    .SetUpdate(true);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            KillTween();

            if (usePunchEffect)
            {
                _currentTween = _rectTransform.DOPunchScale(punchVector, animDuration, 10, 1)
                    .SetUpdate(true);
            }
            else if (useScaleEffect)
            {
                _currentTween = _rectTransform.DOScale(_originalScale * clickScale, animDuration * 0.5f)
                    .SetEase(Ease.OutQuad)
                    .SetUpdate(true);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // 松开鼠标时，如果鼠标还停留在按钮上，恢复到Hover状态；否则恢复原样
            if (eventData.hovered.Contains(gameObject) && useScaleEffect)
            {
                KillTween();
                _currentTween = _rectTransform.DOScale(_originalScale * hoverScale, animDuration)
                    .SetEase(animEase)
                    .SetUpdate(true);
            }
            else
            {
                OnPointerExit(eventData);
            }
        }

        private void KillTween()
        {
            if (_currentTween != null && _currentTween.IsActive())
            {
                _currentTween.Kill();
            }
        }

        private void OnDestroy()
        {
            KillTween();
        }
    }
}