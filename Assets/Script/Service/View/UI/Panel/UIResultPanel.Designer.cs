using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:96b50295-470e-4e7c-889e-5ecb6b17e010
	public partial class UIResultPanel
	{
		public const string Name = "UIResultPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button CloseBtn;
		[SerializeField]
		public UnityEngine.UI.Image TitleImage;
		[SerializeField]
		public TMPro.TextMeshProUGUI ProfitText;
		[SerializeField]
		public TMPro.TextMeshProUGUI ScoreText;
		[SerializeField]
		public UnityEngine.UI.Button NextLevelBtn;
		[SerializeField]
		public UnityEngine.UI.Button TryAgainBtn;
		[SerializeField]
		public UnityEngine.UI.Button MainMenuBtn;
		[SerializeField]
		public UnityEngine.UI.Image UIResultStar_1;
		[SerializeField]
		public UnityEngine.UI.Image UIResultStar_2;
		[SerializeField]
		public UnityEngine.UI.Image UIResultStar_3;
		
		private UIResultPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			CloseBtn = null;
			TitleImage = null;
			ProfitText = null;
			ScoreText = null;
			NextLevelBtn = null;
			TryAgainBtn = null;
			MainMenuBtn = null;
			UIResultStar_1 = null;
			UIResultStar_2 = null;
			UIResultStar_3 = null;
			
			mData = null;
		}
		
		public UIResultPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIResultPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIResultPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
