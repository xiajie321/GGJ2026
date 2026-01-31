using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Script.Service.Model;
using Script.Service.Event;
using Script.Service.Architecture;

namespace Service.View.UI.Panel
{
	public class UIHUDPanelData : UIPanelData
	{
	}
	public partial class UIHUDPanel : UIPanel, IController
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIHUDPanelData ?? new UIHUDPanelData();
			
			this.RegisterEvent<OnMoneyChangedEvent>(e =>
			{
				Gold.text = e.NewMoney.ToString("F0");
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			Gold.text = this.GetModel<LevelModel>().Money.ToString("F0");
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

		public IArchitecture GetArchitecture()
		{
			return GameArchitecture.Interface;
		}
	}
}
