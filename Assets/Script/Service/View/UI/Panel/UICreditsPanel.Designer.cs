using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:669cc053-40ed-48d6-8f5f-d9ac43db3647
	public partial class UICreditsPanel
	{
		public const string Name = "UICreditsPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button CloseBtn;
		
		private UICreditsPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			CloseBtn = null;
			
			mData = null;
		}
		
		public UICreditsPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UICreditsPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UICreditsPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
