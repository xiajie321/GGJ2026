using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:42e36e29-158e-463b-9ba9-15e8c98d2f5f
	public partial class UISettingsPanel
	{
		public const string Name = "UISettingsPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Slider MusicSlider;
		[SerializeField]
		public UnityEngine.UI.Slider SoundSlider;
		[SerializeField]
		public UnityEngine.UI.Button LeftArrowBtn;
		[SerializeField]
		public UnityEngine.UI.Button RightArrowBtn;
		[SerializeField]
		public TMPro.TextMeshProUGUI ResText;
		[SerializeField]
		public UnityEngine.UI.Toggle FullScreenToggle;
		
		private UISettingsPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			MusicSlider = null;
			SoundSlider = null;
			LeftArrowBtn = null;
			RightArrowBtn = null;
			ResText = null;
			FullScreenToggle = null;
			
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
