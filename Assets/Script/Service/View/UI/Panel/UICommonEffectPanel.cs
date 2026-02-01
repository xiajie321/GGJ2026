using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Service.View.UI.Panel
{
	public class UICommonEffectPanelData : UIPanelData
	{
	}
	public partial class UICommonEffectPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			SelectFrame.Hide(); 
		}

		protected override void OnClose()
		{
			
		}
		
		public void ShowFrame(Vector3 position, Vector2 size)
		{
			SelectFrame.Show();
			SelectFrame.transform.position = position;
			(SelectFrame.transform as RectTransform).sizeDelta = size;
		}

		public void HideFrame()
		{
			SelectFrame.Hide();
		}
	}
}
