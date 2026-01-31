using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:0d3e5a60-bff8-4b7c-8188-a95e571cf713
	public partial class UICommonEffectPanel
	{
		public const string Name = "UICommonEffectPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image SelectFrame;
		
		private UICommonEffectPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			SelectFrame = null;
			
			mData = null;
		}
		
		public UICommonEffectPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UICommonEffectPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UICommonEffectPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
