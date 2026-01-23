using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class UISpriteAnimator : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("每秒播放的帧数")]
    [SerializeField] private float frameRate = 12f;
    [Tooltip("是否循环播放")]
    [SerializeField] private bool loop = true;
    [Tooltip("启用时自动播放")]
    [SerializeField] private bool playOnEnable = true;
    
    [Header("Resources")]
    [SerializeField] private List<Sprite> sprites;

    [Header("Events")]
    public UnityEvent OnAnimationComplete;

    private Image _targetImage;
    private float _timer;
    private int _currentIndex;
    private bool _isPlaying;

    public float FPS { get => frameRate; set => frameRate = value; }
    public bool Loop { get => loop; set => loop = value; }
    public bool IsPlaying { get => _isPlaying; set => _isPlaying = value; }
    public List<Sprite> SpriteFrames { get => sprites; set => sprites = value; }

    private void Awake()
    {
        _targetImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (playOnEnable)
        {
            Play();
        }
    }

    private void Update()
    {
        if (!_isPlaying || sprites == null || sprites.Count == 0) return;

        _timer += Time.deltaTime;
        float timePerFrame = 1f / frameRate;

        if (_timer >= timePerFrame)
        {
            _timer -= timePerFrame;
            NextFrame();
        }
    }

    public void Play()
    {
        if (sprites == null || sprites.Count == 0) return;
        
        _currentIndex = 0;
        _timer = 0;
        _isPlaying = true;
        UpdateSprite();
    }

    public void Stop()
    {
        _isPlaying = false;
    }

    private void NextFrame()
    {
        _currentIndex++;

        if (_currentIndex >= sprites.Count)
        {
            if (loop)
            {
                _currentIndex = 0;
            }
            else
            {
                _currentIndex = sprites.Count - 1;
                _isPlaying = false;
                OnAnimationComplete?.Invoke();
                return;
            }
        }
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (_targetImage != null && _currentIndex < sprites.Count)
        {
            _targetImage.sprite = sprites[_currentIndex];
        }
    }
}
