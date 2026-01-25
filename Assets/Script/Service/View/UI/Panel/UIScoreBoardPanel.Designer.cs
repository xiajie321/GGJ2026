using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:bd4c7c7b-a10d-44d1-bf40-a2d07177fbdb
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
