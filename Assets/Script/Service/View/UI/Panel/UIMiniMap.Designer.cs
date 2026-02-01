using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	// Generate Id:c034b3e2-adbe-4e33-a1fa-388c518c0b19
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
		[SerializeField]
		public UnityEngine.UI.Slider Progress;
		
		private UIMiniMapData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Area = null;
			CameraViewRect = null;
			Progress = null;
			
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
