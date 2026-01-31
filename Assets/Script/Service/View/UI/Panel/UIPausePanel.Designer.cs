using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:6da690ab-d68a-4b04-987a-7064b30f0eeb
	public partial class UIPausePanel
	{
		public const string Name = "UIPausePanel";
		
		[SerializeField]
		public UnityEngine.UI.Button CloseBtn;
		[SerializeField]
		public UnityEngine.UI.Image TitleImage;
		[SerializeField]
		public UnityEngine.UI.Button ResumeBtn;
		[SerializeField]
		public UnityEngine.UI.Button SettingBtn;
		[SerializeField]
		public UnityEngine.UI.Button QuitBtn;
		
		private UIPausePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			CloseBtn = null;
			TitleImage = null;
			ResumeBtn = null;
			SettingBtn = null;
			QuitBtn = null;
			
			mData = null;
		}
		
		public UIPausePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIPausePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIPausePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
