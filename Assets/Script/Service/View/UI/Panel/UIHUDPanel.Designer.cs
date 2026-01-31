using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:a6657482-a50c-4d13-8eca-7a48c9b4ad4f
	public partial class UIHUDPanel
	{
		public const string Name = "UIHUDPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button PauseBtn;
		/// <summary>
		/// 用于显示积分
		/// </summary>
		[SerializeField]
		public TMPro.TextMeshProUGUI Gold;
		
		private UIHUDPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			PauseBtn = null;
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
