using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Script.Service.Architecture;
using Script.Service.Model;
using TMPro;

namespace Service.View.UI.Panel
{
	public class UIScoreBoardPanelData : UIPanelData
	{
	}
	public partial class UIScoreBoardPanel : UIPanel, IController
	{
        private GameModel _gameModel;

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIScoreBoardPanelData ?? new UIScoreBoardPanelData();
            // please add init code here

        }

        private void UpdateScoreDisplay(int value)
        {
            if (TextScore != null)
            {
                TextScore.text = $"{value}";
            }
            
        }

        protected override void OnOpen(IUIData uiData = null)
		{
			_gameModel = this.GetModel<GameModel>();

			UpdateScoreDisplay(_gameModel.Points.Value);

			_gameModel.Points.Register(newValue =>
			{
				UpdateScoreDisplay(newValue);
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
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

        public IArchitecture GetArchitecture()
        {
	        return GameArchitecture.Interface;
        }
    }
}
