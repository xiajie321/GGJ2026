using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	public class UIPausePanelData : UIPanelData
	{
	}
	public partial class UIPausePanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIPausePanelData ?? new UIPausePanelData();
			
			ResumeBtn.BindGlobalSelectFrame();
			SettingBtn.BindGlobalSelectFrame();
			QuitBtn.BindGlobalSelectFrame();
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
