using System;
using QFramework;
using Script.SODataScript.MouseCursorSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.Service.View.UI.Panel
{
	/// <summary>
	/// 鼠标指针面板数据类
	/// </summary>
	public class UIMouseCursorPanelData : UIPanelData
	{
	}

	/// <summary>
	/// 鼠标指针面板类，负责自定义鼠标指针的显示逻辑、动画控制及事件分发
	/// </summary>
	public partial class UIMouseCursorPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIMouseCursorPanelData ?? new UIMouseCursorPanelData();
			// 初始化逻辑
		}

		protected override void OnOpen(IUIData uiData = null)
		{
			// 将内置鼠标事件 1:1 映射到对应的视觉状态切换逻辑
			_enter += () => SetState(CursorState.Hover);
			_exit += () => SetState(CursorState.Default);
			_down += () => SetState(CursorState.Down);
			_up += () => SetState(CursorState.Up);
			_move += (dir) => SetState(CursorState.Move);

			// 游戏启动时，如果默认状态配置不为空且包含有效资源，则自动启动默认状态
			if (mConfig != null && mConfig.DefaultState != null)
			{
				if (mConfig.DefaultState.Animator != null || mConfig.DefaultState.Sprite != null)
				{
					SetState(CursorState.Default);
				}
			}
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

		/// <summary>
		/// 直接设置鼠标指针的静态精灵图
		/// </summary>
		/// <param name="sprite">精灵图引用</param>
		public void SetCursorIcon(Sprite sprite)
		{
			spriteAnimation.IsPlaying = false; // 切换到精灵图时停止动画播放
			MouseCursor.sprite = sprite;
		}

		/// <summary>
		/// 当前使用的鼠标指针配置资源
		/// </summary>
		[SerializeField] private CursorConfig mConfig;

		/// <summary>
		/// 设置并应用完整的鼠标指针配置资源
		/// </summary>
		/// <param name="config">配置资源对象</param>
		public void SetConfig(CursorConfig config)
		{
			mConfig = config;
			SetState(CursorState.Default); // 应用配置后重置为默认状态
		}

		/// <summary>
		/// 鼠标指针可能的视觉状态枚举
		/// </summary>
		private enum CursorState
		{
			Default, // 默认闲置状态
			Hover,   // 悬停在 UI 元素上方
			Down,    // 鼠标左键按下
			Up,      // 鼠标左键抬起
			Move     // 鼠标正在移动
		}

		/// <summary>
		/// 当前活跃的状态
		/// </summary>
		private CursorState _currentState = CursorState.Default;

		/// <summary>
		/// 切换鼠标指针到指定状态，并根据配置更新视觉表现
		/// </summary>
		/// <param name="state">目标状态</param>
		private void SetState(CursorState state)
		{
			if (mConfig == null) return;
			
			// 如果目标状态与当前状态相同且不是默认状态，则忽略（默认状态允许强制刷新）
			if (_currentState == state && state != CursorState.Default) return;

			CursorStateConfig stateConfig = GetStateConfig(state);

			// 回退逻辑：如果目标状态未配置任何视觉资源，则自动尝试切换回默认状态
			if (state != CursorState.Default && (stateConfig == null || (stateConfig.Animator == null && stateConfig.Sprite == null)))
			{
				SetState(CursorState.Default);
				return;
			}

			_currentState = state;

			if (stateConfig == null) return;

			// 动画优先级高于静态精灵图
			if (stateConfig.Animator != null)
			{
				SetCursorAnimator(stateConfig.Animator);
			}
			else if (stateConfig.Sprite != null)
			{
				SetCursorIcon(stateConfig.Sprite);
			}
		}

		/// <summary>
		/// 根据状态获取对应的配置项
		/// </summary>
		private CursorStateConfig GetStateConfig(CursorState state)
		{
			if (mConfig == null) return null;
			return state switch
			{
				CursorState.Default => mConfig.DefaultState,
				CursorState.Hover => mConfig.HoverState,
				CursorState.Down => mConfig.DownState,
				CursorState.Up => mConfig.UpState,
				CursorState.Move => mConfig.MoveState,
				_ => mConfig.DefaultState
			};
		}

		/// <summary>
		/// 动态设置默认状态的静态精灵图
		/// </summary>
		public void SetDefaultStateConfig(Sprite sprite)
		{
			EnsureConfig();
			mConfig.DefaultState.Sprite = sprite;
			mConfig.DefaultState.Animator = null;
			if (_currentState == CursorState.Default)
			{
				SetCursorIcon(sprite);
			}
		}

		/// <summary>
		/// 动态设置默认状态的序列帧动画
		/// </summary>
		public void SetDefaultStateConfig(SpriteAnimator animator)
		{
			EnsureConfig();
			mConfig.DefaultState.Animator = animator;
			mConfig.DefaultState.Sprite = null;
			if (_currentState == CursorState.Default)
			{
				SetCursorAnimator(animator);
			}
		}

		/// <summary>
		/// 确保配置资源对象存在，若不存在则创建一个运行时实例
		/// </summary>
		private void EnsureConfig()
		{
			if (mConfig == null)
			{
				mConfig = ScriptableObject.CreateInstance<CursorConfig>();
				mConfig.DefaultState = new CursorStateConfig();
				mConfig.HoverState = new CursorStateConfig();
				mConfig.DownState = new CursorStateConfig();
				mConfig.UpState = new CursorStateConfig();
				mConfig.MoveState = new CursorStateConfig();
			}
		}

		/// <summary>
		/// 动画播放器组件引用
		/// </summary>
		[SerializeField] private UISpriteAnimation spriteAnimation; 

		/// <summary>
		/// 设置并开始播放鼠标指针的序列帧动画
		/// </summary>
		/// <param name="spriteAnimator">动画配置资源</param>
		public void SetCursorAnimator(SpriteAnimator spriteAnimator)
		{
			if (spriteAnimator == null || spriteAnimator.Sprites == null || spriteAnimator.Sprites.Count == 0)
			{
				spriteAnimation.IsPlaying = false;
				spriteAnimation.SpriteFrames.Clear();
				return;
			}

			spriteAnimation.SpriteFrames = spriteAnimator.Sprites;
			spriteAnimation.FPS = spriteAnimator.FPS;
			spriteAnimation.Loop = spriteAnimator.Loop;
			spriteAnimation.Play();
		}

		/// <summary>
		/// 设置鼠标指针相对于屏幕原始坐标的偏移
		/// </summary>
		public void SetCursorOffset(Vector2 offset)
		{
			_mouseCursorOffset = offset;
		}

		/// <summary>
		/// 获取鼠标指针的 Transform 组件
		/// </summary>
		public Transform GetCursorTransform() => MouseCursor.transform;

		/// <summary>
		/// 鼠标指针的视觉偏移量
		/// </summary>
		private Vector2 _mouseCursorOffset = new Vector2(0, 0);

		// 事件订阅管理
		public void AddMouseEnterEvent(Action action) => _enter += action;
		public void RemoveMouseEnterEvent(Action action) => _enter -= action;
		public void AddMouseExitEvent(Action action) => _exit += action;
		public void RemoveMouseExitEvent(Action action) => _exit -= action;
		public void AddMouseDownEvent(Action action) => _down += action;
		public void RemoveMouseDownEvent(Action action) => _down -= action;
		public void AddMouseUpEvent(Action action) => _up += action;
		public void RemoveMouseUpEvent(Action action) => _up -= action;
		public void AddMouseMoveEvent(Action<Vector2> action) => _move += action;
		public void RemoveMouseMoveEvent(Action<Vector2> action) => _move -= action;

		private Action _enter;
		private Action _exit;
		private Action<Vector2> _move;
		private Action _down;
		private Action _up;

		/// <summary>
		/// 记录当前鼠标是否处于 UI 元素上方
		/// </summary>
		private bool _isEnter = true;

		/// <summary>
		/// 记录上一帧的鼠标位置
		/// </summary>
		private Vector3 _lastMousePosition;

		/// <summary>
		/// 移动状态的计时器，用于处理移动停止后的回退
		/// </summary>
		private float _moveTimer;

		/// <summary>
		/// 判定鼠标停止移动所需的静止时间
		/// </summary>
		private const float MOVE_IDLE_TIME = 0.1f;

		private void Update()
		{
			if(!MouseCursor.gameObject.activeInHierarchy) return;
			Vector3 currentMousePosition = Input.mousePosition;
			// 同步自定义光标位置
			MouseCursor.transform.position = new Vector3(currentMousePosition.x + _mouseCursorOffset.x, currentMousePosition.y + _mouseCursorOffset.y, 0);
				
			// 检测鼠标点击事件
			if (Input.GetMouseButtonDown(0)) _down?.Invoke();
			if (Input.GetMouseButtonUp(0)) _up?.Invoke();

			// 检测鼠标进入/离开 UI 区域
			bool isOverUI = EventSystem.current.IsPointerOverGameObject();
			if (isOverUI && _isEnter)
			{
				_enter?.Invoke();
				_isEnter = false;
			}
			else if (!isOverUI && !_isEnter)
			{
				_exit?.Invoke();
				_isEnter = true;
			}

			// 检测鼠标移动
			if (currentMousePosition != _lastMousePosition)
			{
				_moveTimer = MOVE_IDLE_TIME;
				_move?.Invoke((_lastMousePosition - currentMousePosition).normalized);
				_lastMousePosition = currentMousePosition;
			}
			else if (_moveTimer > 0)
			{
				_moveTimer -= Time.deltaTime;
				if (_moveTimer <= 0)
				{
					// 移动停止：根据当前是否悬停在 UI 上，回退到 Hover 或 Default 状态
					if (isOverUI) SetState(CursorState.Hover);
					else SetState(CursorState.Default);
				}
			}

			// Up 状态自动回退逻辑：点击释放动画播放完毕后，自动切回基础状态
			if (_currentState == CursorState.Up)
			{
				if (!spriteAnimation.IsPlaying)
				{
					if (isOverUI) SetState(CursorState.Hover);
					else SetState(CursorState.Default);
				}
			}
		}
	}
}
