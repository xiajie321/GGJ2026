using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:dfc62f29-3f1a-457f-a76d-b5b0a478acd0
	public partial class UIPausePanel
	{
		public const string Name = "UIPausePanel";
		
		
		private UIPausePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UIPausePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIPausePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIPausePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
