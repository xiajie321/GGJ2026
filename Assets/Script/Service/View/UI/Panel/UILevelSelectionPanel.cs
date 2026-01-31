using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	public class UILevelSelectionPanelData : UIPanelData
	{
	}
	public partial class UILevelSelectionPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UILevelSelectionPanelData ?? new UILevelSelectionPanelData();
			// please add init code here

			UIKit.OpenPanel<UICommonEffectPanel>();
			UILevelBtn.BindGlobalSelectFrame();
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
