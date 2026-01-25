using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	public class UIHomePanelData : UIPanelData
	{
	}
	public partial class UIHomePanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIHomePanelData ?? new UIHomePanelData();
			// please add init code here

			BtnStart.onClick.AddListener(() =>
			{
				Debug.Log("开始游戏");
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
