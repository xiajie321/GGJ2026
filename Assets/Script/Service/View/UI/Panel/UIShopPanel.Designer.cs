using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:8e154805-3259-4715-b887-9f4324d461a7
	public partial class UIShopPanel
	{
		public const string Name = "UIShopPanel";
		
		[SerializeField]
		public RectTransform ShopContainer;
		[SerializeField]
		public UnityEngine.UI.Image Shop_1;
		[SerializeField]
		public UnityEngine.UI.Image Shop_2;
		[SerializeField]
		public UnityEngine.UI.Image Shop_3;
		[SerializeField]
		public UnityEngine.UI.Image Shop_4;
		[SerializeField]
		public UnityEngine.UI.Image Shop_5;
		[SerializeField]
		public UnityEngine.UI.Image Shop_6;
		[SerializeField]
		public UnityEngine.UI.Button IllustratedGuideBtn;
		
		private UIShopPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ShopContainer = null;
			Shop_1 = null;
			Shop_2 = null;
			Shop_3 = null;
			Shop_4 = null;
			Shop_5 = null;
			Shop_6 = null;
			IllustratedGuideBtn = null;
			
			mData = null;
		}
		
		public UIShopPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIShopPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIShopPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
