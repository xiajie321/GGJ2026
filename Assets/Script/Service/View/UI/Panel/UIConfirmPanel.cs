using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	public class UIConfirmPanelData : UIPanelData
	{
		public Action OnConfirm;
	}
	public partial class UIConfirmPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIConfirmPanelData ?? new UIConfirmPanelData();
			
			ConfirmBtn.BindGlobalSelectFrame();
			CancelBtn.BindGlobalSelectFrame();
			
			ConfirmBtn.onClick.AddListener(() =>
			{
				mData.OnConfirm?.Invoke();
				CloseSelf();
			});
			
			CancelBtn.onClick.AddListener(() =>
			{
				CloseSelf();
			});
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
