using Cysharp.Threading.Tasks;
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
		private async UniTask Run(UIPanel panel)
		{
			await UniTask.Delay(500);
			UIKit.OpenPanel<UIHUDPanel>();
			this.GetSystem<LevelSystem>().StartLevel(0); // 默认关卡
			await UniTask.Yield();
			UIKit.ClosePanel(panel);
			
		}
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIHomePanelData ?? new UIHomePanelData();

			BtnStart.onClick.AddListener(() =>
			{
				CloseSelf();
				
				Debug.Log("[UIHomePanel] 开始游戏...");
				
				// 重置关卡索引
				Script.Service.View.Game.EnemyExitMono.Index = 1;

				this.GetSystem<SceneSwitchSystem>().LoadSceneAsync<UILoadingPanel>("Level1", v =>
				{
					AudioKit.PlayMusic("Level1_BGM");
					Run(v).Forget();
				});


			});
			BtnSettings.onClick.AddListener(() => UIKit.OpenPanel<UISettingsPanel>());
			BtnCredits.onClick.AddListener(() => UIKit.OpenPanel<UICreditsPanel>());
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
			BtnCredits.BindGlobalSelectFrame();
			BtnExit.BindGlobalSelectFrame();
			
			// 播放背景音乐
			AudioKit.PlayMusic("MainMenu_BGM");
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
