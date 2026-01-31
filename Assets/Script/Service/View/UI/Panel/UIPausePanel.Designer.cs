using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:d63c9223-c717-4a59-8e5d-d46f8fcc8cfa
	public partial class UIPausePanel
	{
		public const string Name = "UIPausePanel";
		
		[SerializeField]
		public UnityEngine.UI.Button CloseBtn;
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
