using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:d9c99887-6118-48ef-976c-b966060e17e9
	public partial class UIHUDPanel
	{
		public const string Name = "UIHUDPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button PauseBtn;
		
		private UIHUDPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			PauseBtn = null;
			
			mData = null;
		}
		
		public UIHUDPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHUDPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHUDPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
