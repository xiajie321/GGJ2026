using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:7fb8a864-e24e-47a0-b3c5-9ce04aaf02f9
	public partial class UISettingsPanel
	{
		public const string Name = "UISettingsPanel";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TextSettings;
		[SerializeField]
		public UnityEngine.UI.Image Dropdown1;
		[SerializeField]
		public UnityEngine.UI.Image Dropdown2;
		[SerializeField]
		public UnityEngine.UI.Image Dropdown3;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UISettingsPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TextSettings = null;
			Dropdown1 = null;
			Dropdown2 = null;
			Dropdown3 = null;
			BtnClose = null;
			
			mData = null;
		}
		
		public UISettingsPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UISettingsPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UISettingsPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
