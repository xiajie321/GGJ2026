using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	public class UICreditsPanelData : UIPanelData
	{
	}
	public partial class UICreditsPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UICreditsPanelData ?? new UICreditsPanelData();
			
			CloseBtn.onClick.AddListener(() => this.CloseSelf());
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
