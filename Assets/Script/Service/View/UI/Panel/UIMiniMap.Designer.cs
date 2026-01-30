using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:8ea72033-ec8e-4dc9-852d-dc5ba5f2f3e7
	public partial class UIMiniMap
	{
		public const string Name = "UIMiniMap";
		
		/// <summary>
		/// 场地bg区域的缩略图
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Image Area;
		/// <summary>
		/// 相机的矩形
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Image CameraViewRect;
		
		private UIMiniMapData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Area = null;
			CameraViewRect = null;
			
			mData = null;
		}
		
		public UIMiniMapData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIMiniMapData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIMiniMapData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
