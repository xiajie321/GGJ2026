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
        
        // 用于保存设置的 Key
        private const string PREFS_RES_INDEX = "Settings_ResIndex";
        private const string PREFS_FULLSCREEN = "Settings_IsFullScreen";

        private int mCurrentResIndex = 2;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UISettingsPanelData ?? new UISettingsPanelData();
            
            MusicSlider.value = AudioKit.Settings.MusicVolume.Value;
            SoundSlider.value = AudioKit.Settings.SoundVolume.Value;
            
            MusicSlider.onValueChanged.AddListener(volume => 
            {
                AudioKit.Settings.MusicVolume.Value = volume;
            });

            SoundSlider.onValueChanged.AddListener(volume => 
            {
                AudioKit.Settings.SoundVolume.Value = volume;
            });
            
            mCurrentResIndex = PlayerPrefs.GetInt(PREFS_RES_INDEX, 2);
            bool isFullScreen = PlayerPrefs.GetInt(PREFS_FULLSCREEN, 1) == 1;
            
            if (mCurrentResIndex >= mResolutions.Count || mCurrentResIndex < 0)
            {
                mCurrentResIndex = mResolutions.Count - 1;
            }

            if (FullScreenToggle != null)
            {
                FullScreenToggle.isOn = isFullScreen;
                FullScreenToggle.onValueChanged.AddListener(isOn =>
                {
                    ApplyResolution();
                    PlayerPrefs.SetInt(PREFS_FULLSCREEN, isOn ? 1 : 0);
                });
            }
            
            UpdateResText(); 
            
            LeftArrowBtn.onClick.AddListener(() => 
            {
                mCurrentResIndex--;
                if (mCurrentResIndex < 0) mCurrentResIndex = mResolutions.Count - 1;
                
                UpdateResText();     // 先更新文字
                ApplyResolution();   // 应用设置
                SaveSettings();      // 保存到注册表
            });

            RightArrowBtn.onClick.AddListener(() => 
            {
                mCurrentResIndex++;
                if (mCurrentResIndex >= mResolutions.Count) mCurrentResIndex = 0;
                
                UpdateResText();
                ApplyResolution();
                SaveSettings();
            });
            
            BtnClose.onClick.AddListener(() => this.CloseSelf());
            
            BtnClose.BindGlobalSelectFrame();
            LeftArrowBtn.BindGlobalSelectFrame();
            RightArrowBtn.BindGlobalSelectFrame();
            if (FullScreenToggle != null)
            {
                // FullScreenToggle.GetComponentInChildren<Button>()?.BindGlobalSelectFrame();
            }
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            base.OnOpen(uiData);
            // 每次打开面板时，确保 UI 状态和当前实际设置一致
            MusicSlider.value = AudioKit.Settings.MusicVolume.Value;
            SoundSlider.value = AudioKit.Settings.SoundVolume.Value;
        }

        protected override void OnClose()
        {
            PlayerPrefs.Save();
        }

        private void ApplyResolution()
        {
            var res = mResolutions[mCurrentResIndex];
            // 获取全屏状态，如果 Toggle 没赋值则默认为 true
            bool isFullScreen = FullScreenToggle != null && FullScreenToggle.isOn;
            
            // 设置分辨率和全屏模式
            Screen.SetResolution(res.x, res.y, isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
            
            Debug.Log($"应用分辨率: {res.x} x {res.y}, 全屏: {isFullScreen}");
        }

        private void SaveSettings()
        {
            PlayerPrefs.SetInt(PREFS_RES_INDEX, mCurrentResIndex);
            // 全屏状态已经在 Toggle 回调里保存了，这里主要是为了保存分辨率索引
            PlayerPrefs.Save();
        }

        private void UpdateResText()
        {
            if (ResText != null)
            {
                var res = mResolutions[mCurrentResIndex];
                ResText.text = $"{res.x} x {res.y}";
            }
        }
    }
}