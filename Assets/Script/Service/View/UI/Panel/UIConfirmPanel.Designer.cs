using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:321d9eb9-6884-4ccc-aad5-2fe54118c55c
	public partial class UIConfirmPanel
	{
		public const string Name = "UIConfirmPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button ConfirmBtn;
		[SerializeField]
		public UnityEngine.UI.Button CancelBtn;
		
		private UIConfirmPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ConfirmBtn = null;
			CancelBtn = null;
			
			mData = null;
		}
		
		public UIConfirmPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIConfirmPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIConfirmPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
