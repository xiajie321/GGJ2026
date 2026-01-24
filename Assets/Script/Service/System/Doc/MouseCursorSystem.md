# MouseCursorSystem (鼠标指针系统) 使用说明文档

`MouseCursorSystem` 是一个基于 QFramework 架构的自定义鼠标指针管理系统。它允许开发者通过配置文件轻松自定义鼠标在不同交互状态下的视觉表现（如静态图标或序列帧动画），并提供了一套完善的事件回调机制。

## **系统架构**

系统由以下几个核心部分组成：

- **MouseCursorSystem.cs**: 系统入口，负责初始化和对外提供控制接口。
- **UIMouseCursorPanel.cs**: UI 层表现，负责自定义光标的实时位置同步、状态切换逻辑以及视觉呈现。
- **CursorConfig.cs**: 基于 ScriptableObject 的配置文件，定义了不同状态下的视觉资源及行为参数。
- **SpriteAnimator.cs**: 序列帧动画配置资源，定义动画帧、帧率及是否循环。

---

## **核心功能**

1.  **自动隐藏系统鼠标**: 在非编辑器模式下，系统会自动隐藏 Unity 默认的系统鼠标指针。
2.  **状态化视觉管理**: 支持五种基础状态：
    - `Default`: 默认空闲状态（静止行为）。
    - `Hover`: 鼠标悬停在 UI 元素上方。
    - `Down`: 鼠标左键按下时。
    - `Up`: 鼠标左键抬起时。
    - `Move`: 鼠标移动过程中。
3.  **多表现支持**: 每个状态均可配置 **静态精灵图 (Sprite)** 或 **序列帧动画 (SpriteAnimator)**。
    - 优先级：如果配置了动画且有效（帧数>0），优先播放动画；否则使用静态精灵图；如果都为空，则保持当前显示但切换逻辑状态。
4.  **动态移动效果**: 支持根据鼠标移动速度动态调整 Move 状态的动画播放速度（FPS）。
5.  **事件扩展**: 提供进入/离开 UI、按下/抬起、移动等事件的订阅接口。

---

## **状态逻辑详解**

### **状态定义与优先级**

系统内部通过有限状态机管理状态，基本优先级和覆盖规则如下：

1.  **Default (默认)**: 基础状态，鼠标静止且未与UI交互时。
2.  **Hover (悬停)**: 鼠标进入UI区域时触发。
    - 优先级高于 Move 状态（悬浮在UI上移动时不会触发 Move 状态）。
3.  **Down (按下)**: 鼠标按下时触发。
    - 在触发 Up 前不会退出该状态。
4.  **Up (抬起)**: 鼠标松开时触发。
    - 这是一个瞬态或短时状态。如果配置了动画，会等待动画播放完毕；否则在下一帧结束。
    - 结束后会自动根据当前是否在UI上回退到 `Hover` 或 `Default` 状态。
5.  **Move (移动)**: 鼠标移动超过一定阈值时触发。
    - 仅在 Default 状态下触发。
    - 如果配置为空，不会切换到此状态。

### **状态切换规则**

- **悬浮UI**: 进入 Hover 状态。
- **点击交互**: 在任意状态下点击，进入 Down 状态；松开进入 Up 状态。
- **移动判定**: 在非UI悬浮、非按下状态下，如果移动距离超过 `MoveThreshold`，切换到 Move 状态。
- **静止判定**: 在 Move 状态下，如果停止移动超过 `IdleThreshold` 时间，自动回退到 Default 状态。

---

## **配置参数说明 (CursorConfig)**

在 Project 窗口创建 `CursorConfig` 后，可以配置以下参数：

### **状态资源**

- `DefaultState`: 默认状态资源。
- `HoverState`: 悬停状态资源。
- `DownState`: 按下状态资源。
- `UpState`: 抬起状态资源。
- `MoveState`: 移动状态资源。

### **行为参数**

- **IdleThreshold (静止阈值)**: 判断鼠标是否停止移动的时间阈值（秒）。
- **MoveThreshold (移动阈值)**: 切换到移动状态所需的最小移动距离（像素）。

### **动态 FPS 配置**

用于控制 Move 状态下动画播放速度随鼠标移动速度变化的效果：

- **EnableSpeedBasedAnim**: 是否启用基于速度的 FPS 调整。
- **MinSpeedThreshold**: 最小速度参考值（像素/秒）。
- **MaxSpeedThreshold**: 最大速度参考值（像素/秒）。
- **MinFPS**: 当移动速度接近或低于最小参考值时，动画播放的 FPS。
- **MaxFPS**: 当移动速度接近或高于最大参考值时，动画播放的 FPS。
    - 系统会根据当前实时速度在 Min/Max 速度参考值之间进行插值，从而在 Min/Max FPS 之间动态调整动画速率。

---

## **使用指南**

### **1. 创建视觉资源**

1.  **创建动画 (可选)**: 在 Project 窗口右键 `Create -> MouseCursorSystem -> UISpriteAnimator`。
    - 设置 `Sprites` 帧列表、`FPS` 和 `Loop`。
2.  **创建配置**: 在 Project 窗口右键 `Create -> MouseCursorSystem -> UICursorConfig`。
    - 将上述动画或静态 Sprite 拖入对应的状态槽位中。
    - 调整阈值和 FPS 参数以获得最佳手感。

### **2. 应用配置**

在代码中通过 `MouseCursorSystem` 应用配置：

```csharp
var cursorSystem = this.GetSystem<MouseCursorSystem>();
cursorSystem.SetCursorConfig(yourConfig);
```

### **3. 常用 API 接口**

| 接口方法 | 说明 |
| :--- | :--- |
| `SetCursorConfig(CursorConfig config)` | 设置并应用完整的状态配置资源 |
| `SetDefaultCursorIcon(Sprite sprite)` | 设置默认状态的鼠标指针图标 |
| `SetDefaultCursorAnimator(SpriteAnimator animator)` | 设置默认状态的鼠标指针动画 |
| `SetCursorOffset(Vector2 offset)` | 设置光标相对于物理坐标的偏移 |
| `ShowMouseCursor(bool show)` | 显示或隐藏自定义光标 |
| `GetCursorTransform()` | 获取鼠标指针的 Transform 组件 |
| `AddMouseEnterEvent(Action action)` | 订阅鼠标进入 UI 区域事件 |
| `RemoveMouseEnterEvent(Action action)` | 移除鼠标进入 UI 区域事件订阅 |
| `AddMouseExitEvent(Action action)` | 订阅鼠标离开 UI 区域事件 |
| `RemoveMouseExitEvent(Action action)` | 移除鼠标离开 UI 区域事件订阅 |
| `AddMouseDownEvent(Action action)` | 订阅鼠标左键按下事件 |
| `RemoveMouseDownEvent(Action action)` | 移除鼠标左键按下事件订阅 |
| `AddMouseUpEvent(Action action)` | 订阅鼠标左键抬起事件 |
| `RemoveMouseUpEvent(Action action)` | 移除鼠标左键抬起事件订阅 |
| `AddMouseMoveEvent(Action<Vector2> action)`| 订阅鼠标移动事件（参数为移动方向） |
| `RemoveMouseMoveEvent(Action<Vector2> action)`| 移除鼠标移动事件订阅 |

---

## **注意事项**

- **UI 交互**: 系统通过 `EventSystem.current.IsPointerOverGameObject()` 判断是否处于 UI 上方。
- **层级管理**: `UIMouseCursorPanel` 默认打开在 `UILevel.PopUI` 层级。
- **调试**: 在控制台可以查看 `[MouseCursorSystem]` 开头的日志，监控状态切换及资源加载情况。
- **资源回退**: 如果某个状态的配置为空（Sprite 和 Animator 都为空），系统会切换逻辑状态，但在视觉上保持上一个有效状态的显示，避免光标消失。
