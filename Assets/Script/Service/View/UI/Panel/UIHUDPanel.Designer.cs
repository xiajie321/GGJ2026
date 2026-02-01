using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:8c2db288-7803-4dd3-8bf7-1a0fbb8b275e
	public partial class UIHUDPanel
	{
		public const string Name = "UIHUDPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button PauseBtn;
		[SerializeField]
		public UnityEngine.UI.Button SpeedBtn;
		/// <summary>
		/// 用于显示积分
		/// </summary>
		[SerializeField]
		public TMPro.TextMeshProUGUI Gold;
		
		private UIHUDPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			PauseBtn = null;
			SpeedBtn = null;
			Gold = null;
			
			mData = null;
		}
		
		public UIHUDPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHUDPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHUDPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
