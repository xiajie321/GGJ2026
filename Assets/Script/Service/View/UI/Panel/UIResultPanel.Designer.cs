using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:cbdd318a-42f8-47c4-913b-6431e2769c65
	public partial class UIResultPanel
	{
		public const string Name = "UIResultPanel";
		
		
		private UIResultPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UIResultPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIResultPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIResultPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
