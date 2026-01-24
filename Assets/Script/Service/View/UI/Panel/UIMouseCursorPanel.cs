using System;
using System.Collections.Generic;
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
		// 动画播放器组件引用
		[SerializeField] private UISpriteAnimation spriteAnimation; 
		
		// 当前使用的鼠标指针配置资源
		[SerializeField] private CursorConfig mConfig;
		
		// 鼠标指针的视觉偏移量
		private Vector2 _mouseCursorOffset = Vector2.zero;

		// 状态枚举
		private enum CursorState
		{
			Default, // 默认静止状态
			Hover,   // 悬停在 UI 元素上方
			Down,    // 鼠标按下
			Up,      // 鼠标抬起
			Move     // 鼠标移动
		}

		// 当前活跃的状态
		private CursorState _currentState = CursorState.Default;
		
		// 记录上一帧的鼠标位置
		private Vector3 _lastMousePosition;
		
		// 移动计时器，用于判断是否静止
		private float _moveTimer;
		
		// 当前是否按下鼠标
		private bool _isMouseDown;
		
		// 当前是否在UI上
		private bool _isOverUI;

		// 上一次所在的UI对象
		private GameObject _lastOverUIObject;

		// 事件委托
		private Action<GameObject> _enter;
		private Action<GameObject> _exit;
		private Action<Vector2> _move;
		private Action _down;
		private Action _up;
		
		// 上一次显示的精灵，用于在配置为空时回退显示
		private Sprite _lastValidSprite;

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIMouseCursorPanelData ?? new UIMouseCursorPanelData();
			
			// 获取初始精灵作为备份
			if (MouseCursor != null)
			{
				_lastValidSprite = MouseCursor.sprite;
			}
			
			// 确保有 Config
			EnsureConfig();
		}

		protected override void OnOpen(IUIData uiData = null)
		{
			// 绑定事件
			_enter += OnMouseEnterUI;
			_exit += OnMouseExitUI;
			_down += OnMouseDownAction;
			_up += OnMouseUpAction;
			_move += OnMouseMoveAction;

			// 初始化状态
			_lastMousePosition = Input.mousePosition;
			UpdateState(CursorState.Default, true);
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
			// 解除绑定
			_enter -= OnMouseEnterUI;
			_exit -= OnMouseExitUI;
			_down -= OnMouseDownAction;
			_up -= OnMouseUpAction;
			_move -= OnMouseMoveAction;
		}

		private void Update()
		{
			if(!MouseCursor.gameObject.activeInHierarchy) return;

			// 1. 同步鼠标位置
			Vector3 currentMousePosition = Input.mousePosition;
			MouseCursor.transform.position = currentMousePosition + (Vector3)_mouseCursorOffset;

			// 2. 检测UI悬浮状态
			bool isOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
			GameObject currentObj = null;
			if (isOverUI)
			{
				currentObj = GetCurrentObjectUnderPointer();
				// 如果已经在UI上，持续更新当前对象，以便Exit时能传递正确的对象
				if (_isOverUI)
				{
					_lastOverUIObject = currentObj;
				}
			}

			if (isOverUI != _isOverUI)
			{
				_isOverUI = isOverUI;
				if (_isOverUI)
				{
					_lastOverUIObject = currentObj;
					_enter?.Invoke(currentObj);
				}
				else
				{
					_exit?.Invoke(_lastOverUIObject);
					_lastOverUIObject = null;
				}
			}

			// 3. 检测鼠标点击
			if (Input.GetMouseButtonDown(0))
			{
				_isMouseDown = true;
				_down?.Invoke();
			}
			else if (Input.GetMouseButtonUp(0))
			{
				_isMouseDown = false;
				_up?.Invoke();
			}

			// 4. 检测鼠标移动
			float dist = Vector3.Distance(currentMousePosition, _lastMousePosition);
			float speed = dist / Time.deltaTime; // 像素/秒

			if (dist > 0)
			{
				Vector2 dir = (currentMousePosition - _lastMousePosition).normalized;
				_move?.Invoke(dir);

				// 只有在 Default 或 Move 状态下，且没有按下鼠标，且不在悬停UI时(需求:悬浮UI移动不触发Move状态)，才考虑切换到 Move 状态
				// 或者是: 需求说"如果光标悬浮在一个UI上: 进入Hover状态(1、如果此时移动不会触发Move状态"
				// 需求说"如果光标没有悬浮在一个UI上:默认处于Default状态,移动时根据能够切换到移动状态的阈值切换到Move状态"
				
				if (!_isOverUI && !_isMouseDown && (_currentState == CursorState.Default || _currentState == CursorState.Move))
				{
					if (dist >= mConfig.MoveThreshold) // 这里阈值可能指的是距离或者速度，假设是帧移动距离
					{
						TrySwitchToMoveState(speed);
					}
				}
				
				_moveTimer = 0; // 重置静止计时器
			}
			else
			{
				_moveTimer += Time.deltaTime;
				// 如果处于 Move 状态且静止时间超过阈值，切回 Default
				if (_currentState == CursorState.Move && _moveTimer >= mConfig.IdleThreshold)
				{
					TrySwitchToIdleState();
				}
			}

			// 5. 更新 Move 状态的动画速度
			if (_currentState == CursorState.Move && mConfig.EnableSpeedBasedAnim && spriteAnimation != null && spriteAnimation.IsPlaying)
			{
				UpdateMoveAnimSpeed(speed);
			}

			// 6. Up 状态结束检测
			if (_currentState == CursorState.Up)
			{
				// 如果动画播放结束（非循环）或者没有动画
				if (spriteAnimation != null && !spriteAnimation.Loop && !spriteAnimation.IsPlaying)
				{
					OnUpStateFinished();
				}
				else if (spriteAnimation == null || !spriteAnimation.enabled) // 只有图片的情况，Up状态可能需要立即结束或持续一帧？
				{
					// 如果只有图片，Up状态在逻辑上可能瞬间完成或者持续到下一次操作。
					// 需求说: "Up状态...动画播放完毕就会...返回"
					// 如果没有动画，通常意味着瞬间切换回之前的状态，或者保持一帧。
					// 这里我们假设如果没有动画，Up状态至少维持一帧然后切回。
					OnUpStateFinished(); 
				}
			}
			
			_lastMousePosition = currentMousePosition;
		}

		private GameObject GetCurrentObjectUnderPointer()
		{
			PointerEventData pointerData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};

			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);

			if (results.Count > 0)
			{
				return results[0].gameObject;
			}
			return null;
		}

		private void OnMouseEnterUI(GameObject obj)
		{
			// 优先级: Hover > Default. Down/Up 优先级更高
			if (_currentState == CursorState.Default || _currentState == CursorState.Move)
			{
				UpdateState(CursorState.Hover);
			}
		}

		private void OnMouseExitUI(GameObject obj)
		{
			// 退出UI，如果在 Hover 状态，切回 Default
			if (_currentState == CursorState.Hover)
			{
				UpdateState(CursorState.Default);
			}
		}

		private void OnMouseDownAction()
		{
			UpdateState(CursorState.Down);
		}

		private void OnMouseUpAction()
		{
			UpdateState(CursorState.Up);
		}

		private void OnMouseMoveAction(Vector2 dir)
		{
			// 具体状态切换逻辑在 Update 中处理
		}

		private void TrySwitchToMoveState(float speed)
		{
			// 检查 MoveState 配置是否有效
			if (mConfig.MoveState == null || (mConfig.MoveState.Sprite == null && mConfig.MoveState.Animator == null))
			{
				// 配置为空时不切换
				return;
			}
			
			UpdateState(CursorState.Move);
		}

		private void TrySwitchToIdleState()
		{
			UpdateState(CursorState.Default);
		}

		private void OnUpStateFinished()
		{
			if (_isOverUI)
			{
				UpdateState(CursorState.Hover);
			}
			else
			{
				UpdateState(CursorState.Default);
			}
		}

		private void UpdateMoveAnimSpeed(float speed)
		{
			// 定义速度参考范围，用于计算 FPS 插值比例 (硬编码参考值)
			const float MIN_SPEED_REF = 100f;
			const float MAX_SPEED_REF = 1000f;

			// 根据速度调整 FPS
			// 使用配置的 MinFPS 和 MaxFPS 进行插值
			float t = Mathf.InverseLerp(MIN_SPEED_REF, MAX_SPEED_REF, speed);

			if (mConfig.MoveState != null && mConfig.MoveState.Animator != null)
			{
				float targetFPS = Mathf.Lerp(mConfig.MinFPS, mConfig.MaxFPS, t); 
				spriteAnimation.FPS = targetFPS;
			}
		}

		/// <summary>
		/// 核心状态切换逻辑
		/// </summary>
		private void UpdateState(CursorState targetState, bool force = false)
		{
			if (_currentState == targetState && !force) return;

			// 优先级检查: 
			// Down 状态下，除非是 Up，否则不切? 需求: "Down状态...在触发Up前都不会退出该状态"
			if (_currentState == CursorState.Down && targetState != CursorState.Up && !force) return;
			
			// Up 状态下，动画未播放完不切? 需求: "Up状态...动画播放完毕就会...返回"
			// 这里由 Update 中的检测逻辑主动触发切换，或者外部强制切换(如再次点击?)
			// 如果在 Up 播放动画时又点击了，可能需要切回 Down?
			if (_currentState == CursorState.Up && targetState == CursorState.Down)
			{
				// 允许 Up -> Down (快速点击)
			}
			else if (_currentState == CursorState.Up && targetState != CursorState.Default && targetState != CursorState.Hover && !force)
			{
				// 正在播放 Up 动画时，忽略 Move 等其他状态
				return;
			}

			// 获取目标配置
			CursorStateConfig config = GetStateConfig(targetState);
			
			// 检查配置是否为空 (增强检查：Animator 必须有帧)
			bool isAnimatorValid = config != null && config.Animator != null && config.Animator.Sprites != null && config.Animator.Sprites.Count > 0;
			bool hasContent = config != null && (config.Sprite != null || isAnimatorValid);
			
			if (!hasContent)
			{
				Debug.Log($"[MouseCursorSystem] State Changed: {_currentState} -> {targetState}. Resources: [Empty/Null]");
				// 如果配置为空且不是 Default，且目标是 Move，则不切换 (需求: MoveState 中的值为空的时候不会切换到移动状态)
				if (targetState == CursorState.Move) return;
				
				// 需求: "CursorStateConfig中Sprite与SpriteAnimator都为空:会正常切换状态但是只会显示默认保存的那个图标"
				// 这里我们将状态切换过去，但在视觉表现上保持或回退
				_currentState = targetState;
				// 保持当前显示不作处理，或者显示 _lastValidSprite ?
				// "显示默认保存的那个图标" -> 可能是指 _lastValidSprite
				if (_lastValidSprite != null)
				{
					ShowSprite(_lastValidSprite);
				}
				return;
			}

			string resourceStatus = hasContent 
				? $"Sprite: {(config.Sprite != null ? config.Sprite.name : "null")}, Animator: {(isAnimatorValid ? $"Set({config.Animator.Sprites.Count} frames)" : "null")}" 
				: "No Resources";

			// 输出状态切换日志
			//Debug.Log($"[MouseCursorSystem] State Changed: {_currentState} -> {targetState}. Resources: [{resourceStatus}]");

			_currentState = targetState;
			ApplyVisual(config);
		}

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
				_ => null
			};
		}

		private void ApplyVisual(CursorStateConfig config)
		{
			// 再次确认 Animator 有效性
			bool isAnimatorValid = config.Animator != null && config.Animator.Sprites != null && config.Animator.Sprites.Count > 0;
			
			if (isAnimatorValid)
			{
				ShowAnimator(config.Animator);
			}
			else if (config.Sprite != null)
			{
				ShowSprite(config.Sprite);
			}
		}

		private void ShowSprite(Sprite sprite)
		{
			if (spriteAnimation != null)
			{
				spriteAnimation.Stop();
				spriteAnimation.enabled = false;
			}
			if (MouseCursor != null)
			{
				MouseCursor.sprite = sprite;
				_lastValidSprite = sprite;
			}
		}

		private void ShowAnimator(SpriteAnimator animConfig)
		{
			if (spriteAnimation == null) return;
			
			spriteAnimation.enabled = true;
			spriteAnimation.SpriteFrames = animConfig.Sprites;
			spriteAnimation.FPS = animConfig.FPS;
			spriteAnimation.Loop = animConfig.Loop;
			spriteAnimation.Play();
			
			// 如果有第一帧，更新 _lastValidSprite
			if (animConfig.Sprites != null && animConfig.Sprites.Count > 0)
			{
				_lastValidSprite = animConfig.Sprites[0];
			}
		}

		// 公共 API
		
		public void SetConfig(CursorConfig config)
		{
			mConfig = config;
			UpdateState(CursorState.Default, true);
		}

		public void SetDefaultStateConfig(Sprite sprite)
		{
			EnsureConfig();
			mConfig.DefaultState.Sprite = sprite;
			mConfig.DefaultState.Animator = null;
			if (_currentState == CursorState.Default) ApplyVisual(mConfig.DefaultState);
		}

		public void SetDefaultStateConfig(SpriteAnimator animator)
		{
			EnsureConfig();
			mConfig.DefaultState.Animator = animator;
			mConfig.DefaultState.Sprite = null;
			if (_currentState == CursorState.Default) ApplyVisual(mConfig.DefaultState);
		}
		
		public void SetCursorIcon(Sprite sprite)
		{
			ShowSprite(sprite);
		}

		public void SetCursorOffset(Vector2 offset)
		{
			_mouseCursorOffset = offset;
		}

		public Transform GetCursorTransform() => MouseCursor != null ? MouseCursor.transform : transform;

		public void AddMouseEnterEvent(Action<GameObject> action) => _enter += action;
		public void RemoveMouseEnterEvent(Action<GameObject> action) => _enter -= action;
		public void AddMouseExitEvent(Action<GameObject> action) => _exit += action;
		public void RemoveMouseExitEvent(Action<GameObject> action) => _exit -= action;
		public void AddMouseDownEvent(Action action) => _down += action;
		public void RemoveMouseDownEvent(Action action) => _down -= action;
		public void AddMouseUpEvent(Action action) => _up += action;
		public void RemoveMouseUpEvent(Action action) => _up -= action;
		public void AddMouseMoveEvent(Action<Vector2> action) => _move += action;
		public void RemoveMouseMoveEvent(Action<Vector2> action) => _move -= action;

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
	}
}
