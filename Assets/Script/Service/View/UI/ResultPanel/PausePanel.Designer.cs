using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:c42f68aa-8c89-4b74-806b-fc425489011b
	public partial class PausePanel
	{
		public const string Name = "PausePanel";
		
		
		private PausePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public PausePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		PausePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PausePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
