using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Script.Service.Architecture;
using UnityEngine.Pool;

namespace Service.View.UI.Panel
{
	public class UIHealthBarPanelData : UIPanelData
	{
	}
	public partial class UIHealthBarPanel : UIPanel, IController
	{
		GameObject _gameObject;
		ObjectPool<UIHealthBarComponent> _objectPool;
		ResLoader _resloader = ResLoader.Allocate();

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIHealthBarPanelData ?? new UIHealthBarPanelData();
			// please add init code here
			_gameObject = _resloader.LoadSync<GameObject>("UIHealthBarComponent");
			_objectPool = new ObjectPool<UIHealthBarComponent>(
				() =>
				{
					UIHealthBarComponent ls = Instantiate(_gameObject).GetComponent<UIHealthBarComponent>();
					ls.transform.SetParent(transform);
					return ls;
				},
				v =>
				{
					v.gameObject.SetActive(true);
				},
				v =>
				{
					v.gameObject.SetActive(false);
				},
				v =>
				{
					Destroy(v.gameObject);
				},
				true,
				10,
				1000);
		}

		UIHealthBarComponent _healthBarComponent;
		public UIHealthBarComponent SetHealth(float current, float max)
		{
			_healthBarComponent = _objectPool.Get();
			_healthBarComponent.SetHealth(current, max);
			return _healthBarComponent;
		}

		public UIHealthBarComponent SetHealth(float current, float max, Color color)
		{
            _healthBarComponent = _objectPool.Get();
            _healthBarComponent.SetHealth(current, max);
			_healthBarComponent.SetFillColor(color);
            return _healthBarComponent;
        }

		public void Release(UIHealthBarComponent component)
		{
			_objectPool.Release(component);
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

        protected override void OnDestroy()
        {
			_resloader.Recycle2Cache();
            base.OnDestroy();
        }

        public IArchitecture GetArchitecture()
        {
			return GameArchitecture.Interface;
        }
    }
}
