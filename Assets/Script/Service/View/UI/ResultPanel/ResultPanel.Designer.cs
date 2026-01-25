using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:fff47598-58e8-4eac-bcf5-01f94228a1e7
	public partial class ResultPanel
	{
		public const string Name = "ResultPanel";
		
		
		private ResultPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public ResultPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ResultPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ResultPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
