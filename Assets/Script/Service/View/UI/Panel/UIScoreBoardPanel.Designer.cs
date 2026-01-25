using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:b2a999c1-ffc3-4981-9546-455a6950f44c
	public partial class UIScoreBoardPanel
	{
		public const string Name = "UIScoreBoardPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image ScoreBoardPanelBg;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextScore;
		
		private UIScoreBoardPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ScoreBoardPanelBg = null;
			TextScore = null;
			
			mData = null;
		}
		
		public UIScoreBoardPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIScoreBoardPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIScoreBoardPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
