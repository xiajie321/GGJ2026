using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:6d2b7844-4c75-47e7-83b9-1b9cca9aed30
	public partial class UILoadingPanel
	{
		public const string Name = "UILoadingPanel";
		
		
		private UILoadingPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UILoadingPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UILoadingPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UILoadingPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
