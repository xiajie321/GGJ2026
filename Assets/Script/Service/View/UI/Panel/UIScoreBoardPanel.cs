    using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Script.Service.Model;
using TMPro;

namespace Service.View.UI.Panel
{
	public class UIScoreBoardPanelData : UIPanelData
	{
	}
	public partial class UIScoreBoardPanel : UIPanel, ICanGetModel
	{
        private GameModel _gameModel;
        private TextMeshProUGUI _textScore;

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIScoreBoardPanelData ?? new UIScoreBoardPanelData();
            // please add init code here
            _gameModel = this.GetModel<GameModel>();

            _textScore = TextScore.GetComponent<TextMeshProUGUI>();
            UpdateScoreDisplay();

            _gameModel.Points.Register(newValue =>
            {
                UpdateScoreDisplay();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void UpdateScoreDisplay()
        {
            if (_textScore != null)
            {
                _textScore.text = $"{_gameModel.Points.Value}";
            }
        }

        protected override void OnOpen(IUIData uiData = null)
		{
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
            throw new System.NotImplementedException();
        }
    }
}
