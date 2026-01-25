using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:9faa845c-615f-4c78-85a1-31dff290c88d
	public partial class TutorialPanel
	{
		public const string Name = "TutorialPanel";
		
		
		private TutorialPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public TutorialPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		TutorialPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new TutorialPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
