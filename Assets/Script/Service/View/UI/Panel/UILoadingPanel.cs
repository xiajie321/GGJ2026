using Cysharp.Threading.Tasks;
using QFramework;
using Script.Service.System;
using UnityEngine;
using UnityEngine.UI;

namespace Service.View.UI.Panel
{
    public class UILoadingPanelData : UIPanelData
    {
    }
    public partial class UILoadingPanel : UIPanel, ISceneSwitch, IController
    {
        public IArchitecture GetArchitecture()
        {
            return Script.Service.Architecture.GameArchitecture.Interface;
        }
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UILoadingPanelData ?? new UILoadingPanelData();
            // please add init code here
        }

        private float _time;//用于判断什么时候关闭窗口
        protected override void OnOpen(IUIData uiData = null)
        {
            _time = 0;
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {

        }

        public void OnLoad(float progress, bool isLoad)
        {
            Debug.Log($"正在加载... 进度: {progress * 100}%");
        }
        
        public bool OnLoadCompleted(float progress, bool isLoad)
        {
            _time += Time.deltaTime;
            if (_time < 1f) return false;
            Debug.Log("加载完成");
            return true;
        }
    }
}
