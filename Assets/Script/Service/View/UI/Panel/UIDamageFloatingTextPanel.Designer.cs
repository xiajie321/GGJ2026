using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:0fefea22-d115-4e10-8a2c-d1a4d631a895
	public partial class UIDamageFloatingTextPanel
	{
		public const string Name = "UIDamageFloatingTextPanel";
		
		
		private UIDamageFloatingTextPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UIDamageFloatingTextPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIDamageFloatingTextPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIDamageFloatingTextPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
