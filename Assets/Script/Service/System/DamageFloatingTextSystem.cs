using Cysharp.Threading.Tasks;
using DG.Tweening;
using QFramework;
using Service.View.UI.Panel;
using UnityEngine;
namespace Script.Service.System
{
    public class DamageFloatingTextSystem:AbstractSystem
    {
        private UIDamageFloatingTextPanel _panel;
        Camera _mainCamera;
        protected override void OnInit()
        {
            _panel = UIKit.OpenPanel<UIDamageFloatingTextPanel>();
            _mainCamera = Camera.main;
        }
        private float _yDistance = 50f; // 动画移动的垂直距离
        private float _animTime = 0.5f; // 单阶段动画持续时间
        private Ease _animEase = Ease.Linear; // 动画缓动类型

        /// <summary>
        /// 播放伤害漂浮文字动画
        /// </summary>
        /// <param name="component">漂浮文字组件</param>
        private void Animation(UIDamageTextComponent component)
        {
            Vector3 position = component.transform.position;
            var text = component.TextComponent;

            // 杀死该组件上正在进行的动画，防止对象池复用时状态冲突
            component.transform.DOKill();
            text.DOKill();

            Sequence sequence = DOTween.Sequence();

            // 初始状态：透明度为0，位置在目标位置下方 _yDistance 处
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            component.transform.position = position + new Vector3(0, -_yDistance, 0);

            // 第一阶段：淡入 (0->1) 并向上移动到目标位置
            sequence.Append(text.DOFade(1, _animTime).SetEase(_animEase));
            sequence.Join(component.transform.DOMove(position, _animTime).SetEase(_animEase));

            // 第二阶段：淡出 (1->0) 并继续向上移动 _yDistance 距离
            sequence.Append(text.DOFade(0, _animTime).SetEase(_animEase));
            sequence.Join(component.transform.DOMove(position + new Vector3(0, _yDistance, 0), _animTime).SetEase(_animEase));

            // 动画完成后将组件回收至对象池
            sequence.OnComplete(() => _panel.Release(component));
            sequence.Play();
        }

        /// <summary>
        /// 设置并显示漂浮文字
        /// </summary>
        /// <param name="text">显示的文字内容</param>
        /// <param name="position">显示的目标位置</param>
        public void SetText(string text, Vector3 position)
        {
            if (_panel == null)
            {
                _panel = UIKit.OpenPanel<UIDamageFloatingTextPanel>(UILevel.PopUI);
            }
            else if (!_panel.gameObject.activeInHierarchy)
            {
                UIKit.OpenPanel<UIDamageFloatingTextPanel>(UILevel.PopUI);
            }
            
            if (_mainCamera == null) _mainCamera = Camera.main;
            if (_mainCamera == null) return;

            var component = _panel.SetText(text);
            component.transform.position = _mainCamera.WorldToScreenPoint(position);
            Animation(component);
        }

        /// <summary>
        /// 设置并显示漂浮文字（带颜色）
        /// </summary>
        /// <param name="text">显示的文字内容</param>
        /// <param name="color">文字颜色</param>
        public void SetText(string text, Color color)
        {
            if (_panel == null)
            {
                _panel = UIKit.OpenPanel<UIDamageFloatingTextPanel>(UILevel.PopUI);
            }
            else if (!_panel.gameObject.activeInHierarchy)
            {
                UIKit.OpenPanel<UIDamageFloatingTextPanel>(UILevel.PopUI);
            }

            if (_mainCamera == null) _mainCamera = Camera.main;
            if (_mainCamera == null) return;

            SetText(text, color,_mainCamera.WorldToScreenPoint(Vector2.zero));
        }

        /// <summary>
        /// 设置并显示漂浮文字（带颜色和位置）
        /// </summary>
        /// <param name="text">显示的文字内容</param>
        /// <param name="color">文字颜色</param>
        /// <param name="position">显示的目标位置</param>
        public void SetText(string text, Color color, Vector2 position)
        {
            if (_panel == null)
            {
                _panel = UIKit.OpenPanel<UIDamageFloatingTextPanel>(UILevel.PopUI);
            }
            else if (!_panel.gameObject.activeInHierarchy)
            {
                UIKit.OpenPanel<UIDamageFloatingTextPanel>(UILevel.PopUI);
            }

            if (_mainCamera == null) _mainCamera = Camera.main;
            if (_mainCamera == null) return;

            var component = _panel.SetText(text, color);
            component.transform.position = _mainCamera.WorldToScreenPoint(position);
            Animation(component);
        }
    }
}
