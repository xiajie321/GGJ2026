using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class UI_Typewriter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 0.05f; // 每个字符的显示间隔
    [SerializeField] private bool autoPlayOnStart = false;
    
    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI textComponent;

    [Header("Events")]
    public UnityEvent OnTypeStart;
    public UnityEvent OnTypeFinish;

    private string _fullText;
    private Tween _typeTween;
    
    public bool IsTyping => _typeTween != null && _typeTween.IsActive() && _typeTween.IsPlaying();

    private void Awake()
    {
        if (textComponent == null) textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (autoPlayOnStart && textComponent != null)
        {
            ShowText(textComponent.text);
        }
    }

    /// <summary>
    /// 开始打字效果
    /// </summary>
    /// <param name="content">要显示的完整文本</param>
    public void ShowText(string content)
    {
        KillTween();

        _fullText = content;
        textComponent.text = ""; 
    
        OnTypeStart?.Invoke();

        float duration = content.Length * speed;
        
        _typeTween = DOVirtual.Int(0, content.Length, duration, (value) => {
                textComponent.text = content.Substring(0, value);
            })
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                OnTypeFinish?.Invoke();
                _typeTween = null;
            });
    }

    /// <summary>
    /// 立即完成打字
    /// </summary>
    public void SkipTypewriter()
    {
        if (IsTyping)
        {
            KillTween();
            textComponent.text = _fullText; // 直接显示最终文本
            OnTypeFinish?.Invoke();
        }
    }

    private void KillTween()
    {
        if (_typeTween != null)
        {
            _typeTween.Kill();
            _typeTween = null;
        }
    }

    private void OnDestroy()
    {
        KillTween();
    }
}