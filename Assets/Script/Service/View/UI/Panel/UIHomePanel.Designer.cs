using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:2b1d1132-e791-4288-8740-ce86562bafde
	public partial class UIHomePanel
	{
		public const string Name = "UIHomePanel";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnStart;
		[SerializeField]
		public UnityEngine.UI.Button BtnSettings;
		[SerializeField]
		public UnityEngine.UI.Button BtnCredits;
		[SerializeField]
		public UnityEngine.UI.Button BtnExit;
		
		private UIHomePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnStart = null;
			BtnSettings = null;
			BtnCredits = null;
			BtnExit = null;
			
			mData = null;
		}
		
		public UIHomePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHomePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHomePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
