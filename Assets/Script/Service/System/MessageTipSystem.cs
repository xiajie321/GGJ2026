using QFramework;
using Script.Service.Model;
using Script.SODataScript.GameConfig;
using Service.View.UI.Panel;
using UnityEngine;

namespace Script.Service.System
{
    public class MessageTipSystem:AbstractSystem
    {
        UIMessageTipPanel _messageTipPanel;
        private void Init()
        {
            if (_messageTipPanel) return;
            _messageTipPanel = UIKit.OpenPanel<UIMessageTipPanel>(UILevel.PopUI);
        }

        protected override void OnInit()
        {
            Init();
            Debug.Log("[MessageTipSystem] 加载完成...");
        }
        /// <summary>
        /// 显示一下的提示
        /// </summary>
        public void ShowTip(string message)
        {
            _messageTipPanel.ShowTip(message);
        }
        /// <summary>
        /// 消息弹窗
        /// </summary>
        public void ShowMessage(string title,string message)
        {
            _messageTipPanel.ShowMessage(title, message);
        }
        /// <summary>
        /// 显示一下的提示
        /// </summary>
        public void ShowTip(string message,Vector2 position)
        {
            _messageTipPanel.ShowTip(message, position);
        }
        /// <summary>
        /// 消息弹窗
        /// </summary>
        public void ShowMessage(string title,string message,Vector2 position)
        {
            _messageTipPanel.ShowMessage(title, message, position);
        }


    }
}