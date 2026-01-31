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
			Debug.Log($"正在加载... 进度: {progress * 100}%");
		}

		public bool OnLoadCompleted(float progress, bool isLoad)
		{
			Debug.Log("加载完成");
			return true;
		}
	}
}
