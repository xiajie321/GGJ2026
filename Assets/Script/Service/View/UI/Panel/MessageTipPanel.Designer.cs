using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:d5b91028-93d7-4a91-98ea-c2307efa8375
	public partial class MessageTipPanel
	{
		public const string Name = "MessageTipPanel";
		
		/// <summary>
		/// 用于控制MessagePanel开关
		/// </summary>
		[SerializeField]
		public UnityEngine.GameObject MessagePanel;
		/// <summary>
		/// 用于制作面板动画
		/// </summary>
		[SerializeField]
		public UnityEngine.RectTransform MessagePanelBg;
		/// <summary>
		/// 标题文本
		/// </summary>
		[SerializeField]
		public TMPro.TextMeshProUGUI MessageTitleText;
		/// <summary>
		/// 文本内容
		/// </summary>
		[SerializeField]
		public TMPro.TextMeshProUGUI MessageText;
		/// <summary>
		/// 退出按钮
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Button MessageExit;
		/// <summary>
		/// 用于制作Tip动画
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Image Tip;
		[SerializeField]
		public TMPro.TextMeshProUGUI TipText;
		
		private MessageTipPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			MessagePanel = null;
			MessagePanelBg = null;
			MessageTitleText = null;
			MessageText = null;
			MessageExit = null;
			Tip = null;
			TipText = null;
			
			mData = null;
		}
		
		public MessageTipPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		MessageTipPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new MessageTipPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
