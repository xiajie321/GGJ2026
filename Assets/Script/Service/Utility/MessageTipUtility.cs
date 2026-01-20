using QFramework;
using Service.View.UI.Panel;
using UnityEngine;

namespace Script.Service.Utility
{
    public class MessageTipUtility:Singleton<MessageTipUtility>
    {
        private MessageTipUtility()
        {
        }

        MessageTipPanel _messageTipPanel;
        /// <summary>
        /// 显示一下的提示
        /// </summary>
        public void ShowTip(string message)
        {
            Init();
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

        public override void OnSingletonInit()
        {
            Init();
        }
        private void Init()
        {
            if (!_messageTipPanel)
            {
                _messageTipPanel = UIKit.OpenPanel<MessageTipPanel>(UILevel.PopUI);
            }
        }
    }
}