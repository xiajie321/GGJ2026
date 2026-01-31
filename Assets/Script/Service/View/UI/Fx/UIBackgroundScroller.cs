using UnityEngine;
using UnityEngine.UI;

namespace Service.View.UI.Component
{
    [RequireComponent(typeof(RawImage))]
    public class UIBackgroundScroller : MonoBehaviour
    {
        [Header("滚动速度")]
        [Tooltip("水平滚动速度，正数向左，负数向右")]
        public float SpeedX = 0.1f;
        [Tooltip("垂直滚动速度")]
        public float SpeedY = 0.0f;

        private RawImage _rawImage;

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
        }

        private void Update()
        {
            Rect uvRect = _rawImage.uvRect;
            
            uvRect.x += SpeedX * Time.deltaTime;
            uvRect.y += SpeedY * Time.deltaTime;
            
            _rawImage.uvRect = uvRect;
        }
    }
}