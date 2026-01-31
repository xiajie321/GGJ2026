using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Script.Service.System;
using UnityEngine.EventSystems;

namespace Service.View.UI.Panel
{
	public class UIHomePanelData : UIPanelData
	{
	}
	public partial class UIHomePanel : UIPanel,IController
	{
		public IArchitecture GetArchitecture()
		{
			return Script.Service.Architecture.GameArchitecture.Interface;
		}
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIHomePanelData ?? new UIHomePanelData();

			BtnStart.onClick.AddListener(() =>
			{
				CloseSelf();
				
				Debug.Log("[UIHomePanel] 开始游戏...");

				this.GetSystem<SceneSwitchSystem>().LoadSceneAsync<UILoadingPanel>("Level1");


			});
			BtnSettings.onClick.AddListener(() => UIKit.OpenPanel<UISettingsPanel>());
			BtnExit.onClick.AddListener(() => 
			{
				// 打开确认面板
				UIKit.OpenPanel<UIConfirmPanel>(new UIConfirmPanelData() 
				{
					OnConfirm = () => 
					{
#if UNITY_EDITOR
						UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
					}
				});
			});
			
			BtnStart.BindGlobalSelectFrame();
			BtnSettings.BindGlobalSelectFrame();
			BtnExit.BindGlobalSelectFrame();
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
