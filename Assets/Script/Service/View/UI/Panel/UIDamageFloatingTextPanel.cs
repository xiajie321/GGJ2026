using UnityEngine;
using QFramework;
using Script.Service.Architecture;
using UnityEngine.Pool;

namespace Service.View.UI.Panel
{
	public class UIDamageFloatingTextPanelData : UIPanelData
	{
	}
	public partial class UIDamageFloatingTextPanel : UIPanel,IController
	{
		ResLoader _resLoader = ResLoader.Allocate();
		GameObject _gameObject;
		ObjectPool<UIDamageTextComponent> _objectPool;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIDamageFloatingTextPanelData ?? new UIDamageFloatingTextPanelData();
			_gameObject =_resLoader.LoadSync<GameObject>("UIDamageTextComponent");
			_objectPool = new ObjectPool<UIDamageTextComponent>(
				() =>
				{
					UIDamageTextComponent ls = Instantiate(_gameObject).GetComponent<UIDamageTextComponent>();
					ls.transform.SetParent(transform);
					return ls;
				},
				v =>
				{
					v.gameObject.SetActive(true);
				}, v =>
				{
					v.gameObject.SetActive(false);
				}, v =>
				{
					Destroy(v.gameObject);
				},
				true,
				10,
				1000);
		}
		UIDamageTextComponent _damageTextComponent;
		public UIDamageTextComponent SetText(string text)
		{
			_damageTextComponent = _objectPool.Get();
			_damageTextComponent.SetText(text);
			return _damageTextComponent;
		}
		public UIDamageTextComponent SetText(string text,Color color)
		{
			_damageTextComponent = _objectPool.Get();
			_damageTextComponent.SetText(text);
			_damageTextComponent.SetColor(color);
			return _damageTextComponent;
		}

		public void Release(UIDamageTextComponent component)
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
			_resLoader.Recycle2Cache();
			base.OnDestroy();
		}

		public IArchitecture GetArchitecture()
		{
			return GameArchitecture.Interface;
		}
	}
}
