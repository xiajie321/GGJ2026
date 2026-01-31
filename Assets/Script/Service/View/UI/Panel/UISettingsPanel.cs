using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;

namespace Service.View.UI.Panel
{
	public class UISettingsPanelData : UIPanelData { }

	public partial class UISettingsPanel : UIPanel
	{
		private List<Vector2Int> mResolutions = new List<Vector2Int>
		{
			new Vector2Int(1280, 720),
			new Vector2Int(1600, 900),
			new Vector2Int(1920, 1080),
			new Vector2Int(2560, 1440)
		};
		private int mCurrentResIndex = 2;

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UISettingsPanelData ?? new UISettingsPanelData();
			
			MusicSlider.onValueChanged.AddListener(volume => 
			{
				Debug.Log($"音乐音量: {volume}");
			});

			SoundSlider.onValueChanged.AddListener(volume => 
			{
				Debug.Log($"音效音量: {volume}");
			});
			
			UpdateResText(); // 初始化显示

			LeftArrowBtn.onClick.AddListener(() => 
			{
				mCurrentResIndex--;
				if (mCurrentResIndex < 0) mCurrentResIndex = mResolutions.Count - 1;
				ApplyResolution();
			});

			RightArrowBtn.onClick.AddListener(() => 
			{
				mCurrentResIndex++;
				if (mCurrentResIndex >= mResolutions.Count) mCurrentResIndex = 0;
				ApplyResolution();
			});
			
			BtnClose.onClick.AddListener(() => this.CloseSelf());
			
			BtnClose.BindGlobalSelectFrame();
			LeftArrowBtn.BindGlobalSelectFrame();
			RightArrowBtn.BindGlobalSelectFrame();
		}

		protected override void OnClose()
		{
			throw new System.NotImplementedException();
		}

		private void ApplyResolution()
		{
			var res = mResolutions[mCurrentResIndex];
			Screen.SetResolution(res.x, res.y, FullScreenMode.Windowed);
			UpdateResText();
			Debug.Log($"切换分辨率至: {res.x} x {res.y}");
		}

		private void UpdateResText()
		{
			var res = mResolutions[mCurrentResIndex];
			ResText.text = $"{res.x} x {res.y}";
		}
	}
}