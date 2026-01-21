using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	public class MessageTipPanelData : UIPanelData
	{
	}
	public partial class UIMessageTipPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as MessageTipPanelData ?? new MessageTipPanelData();
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			MessagePanel.SetActive(false);
			Tip.gameObject.SetActive(false);
			MessageExit.onClick.AddListener(Exit);
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
			MessageExit.onClick.RemoveAllListeners();
		}

		private void Exit()
		{
			MessagePanel.SetActive(false);
		}
		public void ShowMessage(string title,string message)
		{
			ShowMessage(title, message, Vector2.zero);
		}
		public void ShowMessage(string title, string message,Vector2 position)
		{
			MessageTitleText.text = title;
			MessageText.text = message;
			MessagePanelBg.transform.position = position;
			MessagePanel.SetActive(true);
			MessagePanel.transform?.DOKill();
			//TODO [ShowMessage]动画补充
		}

		private float _yDistance = 50;//移动的距离
		private float _animTime = 0.5f;//动画持续时间
		private Ease _animEase = Ease.Linear;
		public void ShowTip(string message)
		{
			ShowTip(message, Vector2.zero);
		}
		private Sequence _sequence;
		public void ShowTip(string message, Vector2 position)
		{
			TipText.text = message;
			Tip.gameObject.SetActive(true);
			
			// 启动动画前使用DOKill()杀死原先的动画
			_sequence?.Kill();
			_sequence = DOTween.Sequence();

			// 初始状态：透明度为0，位置在 position.y - _yDistance
			Tip.color = new Color(Tip.color.r, Tip.color.g, Tip.color.b, 0);
			TipText.color = new Color(TipText.color.r, TipText.color.g, TipText.color.b, 0);
			Tip.transform.position = position + new Vector2(0, -_yDistance);
			
			// 第一阶段：由0到1，由 position.y - _yDistance 到 position.y
			_sequence.Append(Tip.DOFade(1, _animTime).SetEase(_animEase));
			_sequence.Join(TipText.DOFade(1, _animTime).SetEase(_animEase));
			_sequence.Join(Tip.transform.DOMove(position, _animTime).SetEase(_animEase));

			// 第二阶段：由1到0，由 position.y 到 position.y + _yDistance
			_sequence.Append(Tip.DOFade(0, _animTime).SetEase(_animEase));
			_sequence.Join(TipText.DOFade(0, _animTime).SetEase(_animEase));
			_sequence.Join(Tip.transform.DOMove(position + new Vector2(0, _yDistance), _animTime).SetEase(_animEase));

			// 在所有动画播放完毕后使用 Tip.gameObject.SetActive(false)
			_sequence.OnComplete(() => Tip.gameObject.SetActive(false));
			_sequence.Play();
		}
	}
}
