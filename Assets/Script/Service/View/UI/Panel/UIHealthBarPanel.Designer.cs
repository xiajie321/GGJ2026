using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:269ab4e3-af75-4c17-b351-af69a1095d1c
	public partial class UIHealthBarPanel
	{
		public const string Name = "UIHealthBarPanel";
		
		
		private UIHealthBarPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UIHealthBarPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHealthBarPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHealthBarPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
