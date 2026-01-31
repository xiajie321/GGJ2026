using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:22ce3e5f-093a-43b3-a13e-d1d482c342fd
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
		
		private UISettingsPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			MusicSlider = null;
			SoundSlider = null;
			LeftArrowBtn = null;
			RightArrowBtn = null;
			ResText = null;
			
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
