using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;

namespace Service.View.UI.Panel
{
	public class UIHomePanelData : UIPanelData
	{
	}
	public partial class UIHomePanel : UIPanel
	{
		private Transform mSelectFrame;
		
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIHomePanelData ?? new UIHomePanelData();
			
			mSelectFrame = transform.Find("ButtonBox/SelectFrame");
			mSelectFrame.gameObject.SetActive(false);
			
			BtnStart.onClick.AddListener(() => Debug.Log("开始游戏"));
			BtnSettings.onClick.AddListener(() => UIKit.OpenPanel<UISettingsPanel>());
			BtnExit.onClick.AddListener(() => {
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
			});
			
			RegisterHoverEvent(BtnStart.gameObject);
			RegisterHoverEvent(BtnSettings.gameObject);
			RegisterHoverEvent(BtnExit.gameObject);
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

		/// <summary>
		/// 注册鼠标进入和退出事件
		/// </summary>
		private void RegisterHoverEvent(GameObject obj)
		{
			var trigger = obj.GetComponent<EventTrigger>() ?? obj.AddComponent<EventTrigger>();
			
			EventTrigger.Entry enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
			enter.callback.AddListener((data) => {
				mSelectFrame.gameObject.SetActive(true);
				mSelectFrame.position = obj.transform.position;
			});
			trigger.triggers.Add(enter);
			
			EventTrigger.Entry exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
			exit.callback.AddListener((data) => {
				mSelectFrame.gameObject.SetActive(false);
			});
			trigger.triggers.Add(exit);
		}
	}
}
