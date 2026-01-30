using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Script.Service.System;

namespace Service.View.UI.Panel
{
	public class UILoadingPanelData : UIPanelData
	{
	}
	public partial class UILoadingPanel : UIPanel, ISceneSwitch
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UILoadingPanelData ?? new UILoadingPanelData();
			// please add init code here
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

		public void OnLoad(float progress, bool isLoad)
		{
			throw new System.NotImplementedException();
		}

		public bool OnLoadCompleted(float progress, bool isLoad)
		{
			throw new System.NotImplementedException();
		}
	}
}
