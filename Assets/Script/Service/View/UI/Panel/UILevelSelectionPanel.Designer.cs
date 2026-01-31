using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:706b5378-8825-43e6-accd-bda71ca6fcb1
	public partial class UILevelSelectionPanel
	{
		public const string Name = "UILevelSelectionPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button UILevelBtn;
		
		private UILevelSelectionPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			UILevelBtn = null;
			
			mData = null;
		}
		
		public UILevelSelectionPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UILevelSelectionPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UILevelSelectionPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
