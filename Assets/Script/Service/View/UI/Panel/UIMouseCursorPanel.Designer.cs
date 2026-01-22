using UnityEngine;

namespace Script.Service.View.UI.Panel
{
	// Generate Id:f8a165f3-1df3-49b3-85cc-0de9f599c43a
	public partial class UIMouseCursorPanel
	{
		public const string Name = "UIMouseCursorPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image MouseCursor;
		
		private UIMouseCursorPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			MouseCursor = null;
			
			mData = null;
		}
		
		public UIMouseCursorPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIMouseCursorPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIMouseCursorPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
