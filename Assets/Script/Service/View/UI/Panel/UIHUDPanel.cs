using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Script.Service.Model;
using Script.Service.Event;
using Script.Service.Architecture;

namespace Service.View.UI.Panel
{
    public class UIHUDPanelData : UIPanelData
    {
    }

    public partial class UIHUDPanel : UIPanel, IController
    {
        private const float NormalSpeed = 1.0f;
        private const float FastSpeed = 2.0f;
        
        [Header("加速按钮图标")]
        [SerializeField] private Sprite NormalSpeedSprite; // 1x 状态显示的图片
        [SerializeField] private Sprite FastSpeedSprite;   // 2x 状态显示的图片

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIHUDPanelData ?? new UIHUDPanelData();
            
            this.RegisterEvent<OnMoneyChangedEvent>(e =>
            {
                Gold.text = e.NewMoney.ToString("F0");
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            
            PauseBtn.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIPausePanel>();
            });
            PauseBtn.BindGlobalSelectFrame();
            
            SpeedBtn.onClick.AddListener(OnSpeedBtnClick);
            SpeedBtn.BindGlobalSelectFrame();
            
            UpdateSpeedState();
        }
        
        private void OnSpeedBtnClick()
        {
            if (Time.timeScale > NormalSpeed)
            {
                Time.timeScale = NormalSpeed;
            }
            else
            {
                Time.timeScale = FastSpeed;
            }
            
            UpdateSpeedState();
        }
        
        private void UpdateSpeedState()
        {
            if (SpeedBtn.image != null)
            {
                if (Time.timeScale > NormalSpeed)
                {
                    if (FastSpeedSprite != null) 
                        SpeedBtn.image.sprite = FastSpeedSprite;
                }
                else
                {
                    if (NormalSpeedSprite != null) 
                        SpeedBtn.image.sprite = NormalSpeedSprite;
                }
            }
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            Gold.text = this.GetModel<LevelModel>().Money.ToString("F0");
            
            UpdateSpeedState();
        }
        
        protected override void OnClose()
        {
            Time.timeScale = NormalSpeed;
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}