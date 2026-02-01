using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Script.Service.Architecture;
using Script.Service.System;
using Script.Service.View.UI.Panel;

namespace Service.View.UI.Panel
{
    public class UIPausePanelData : UIPanelData
    {
    }

    public partial class UIPausePanel : UIPanel,IController
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIPausePanelData ?? new UIPausePanelData();
            
            ResumeBtn.onClick.AddListener(ResumeGame);
            CloseBtn.onClick.AddListener(ResumeGame);
            
            SettingBtn.onClick.AddListener(() => {
                UIKit.OpenPanel<UISettingsPanel>();
            });
            
            QuitBtn.onClick.AddListener(() => {
                UIKit.OpenPanel<UIConfirmPanel>(new UIConfirmPanelData() {
                    OnConfirm = () => {
                        Time.timeScale = 1f;
                        this.GetSystem<SceneSwitchSystem>().LoadSceneAsync<UILoadingPanel>("GameBoot", v =>
                        {
                            UIKit.CloseAllPanel();
                            UIKit.OpenPanel<UIMouseCursorPanel>(UILevel.PopUI);
                            UIKit.OpenPanel<UIHomePanel>();
                            UIKit.ClosePanel(v);
                        });
                        
                    }
                });
            });
            
            ResumeBtn.BindGlobalSelectFrame();
            SettingBtn.BindGlobalSelectFrame();
            QuitBtn.BindGlobalSelectFrame();
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f;
            CloseSelf();
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            Time.timeScale = 0f;
        }

        protected override void OnClose()
        {
            Time.timeScale = 1f;
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}