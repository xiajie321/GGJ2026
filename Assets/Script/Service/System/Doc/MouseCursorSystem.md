# MouseCursorSystem (鼠标指针系统) 使用说明文档

`MouseCursorSystem` 是一个基于 QFramework 架构的自定义鼠标指针管理系统。它允许开发者通过配置文件轻松自定义鼠标在不同交互状态下的视觉表现（如静态图标或序列帧动画），并提供了一套完善的事件回调机制。

## **系统架构**

系统由以下几个核心部分组成：

- **[MouseCursorSystem.cs](file:///d:/Unity/UnityProject/GGJ2026/Assets/Script/Service/System/MouseCurosrSystem.cs)**: 系统入口，负责初始化和对外提供控制接口。
- **[UIMouseCursorPanel.cs](file:///d:/Unity/UnityProject/GGJ2026/Assets/Script/Service/View/UI/Panel/UIMouseCursorPanel.cs)**: UI 层表现，负责自定义光标的实时位置同步、状态切换逻辑以及视觉呈现。
- **[CursorConfig.cs](file:///d:/Unity/UnityProject/GGJ2026/Assets/Script/SODataScript/MouseCursorSystem/CursorConfig.cs)**: 基于 ScriptableObject 的配置文件，定义了不同状态下的视觉资源。
- **[SpriteAnimator.cs](file:///d:/Unity/UnityProject/GGJ2026/Assets/Script/SODataScript/MouseCursorSystem/SpriteAnimator.cs)**: 序列帧动画配置资源，定义动画帧、帧率及是否循环。

---

## **核心功能**

1.  **自动隐藏系统鼠标**: 在非编辑器模式下，系统会自动隐藏 Unity 默认的系统鼠标指针。
2.  **状态化视觉管理**: 支持五种基础状态：
    - `Default`: 默认空闲状态。
    - `Hover`: 鼠标悬停在 UI 元素（具有 `Raycast Target` 的 UI）上方。
    - `Down`: 鼠标左键按下时。
    - `Up`: 鼠标左键抬起时。
    - `Move`: 鼠标移动过程中。
3.  **多表现支持**: 每个状态均可配置 **静态精灵图 (Sprite)** 或 **序列帧动画 (SpriteAnimator)**。动画优先级高于静态精灵。
4.  **实时位置同步**: 自定义光标会自动同步物理鼠标位置，并支持设置全局偏移（`Offset`）。
5.  **事件扩展**: 提供进入/离开 UI、按下/抬起、移动等事件的订阅接口。

---

## **使用指南**

### **1. 创建视觉资源**

1.  **创建动画 (可选)**: 在 Project 窗口右键 `Create -> MouseCursorSystem -> UISpriteAnimator`。
    - 设置 `Sprites` 帧列表、`FPS` 和 `Loop`。
2.  **创建配置**: 在 Project 窗口右键 `Create -> MouseCursorSystem -> UICursorConfig`。
    - 将上述动画或静态 Sprite 拖入对应的状态槽位中（如 `HoverState`）。

### **2. 应用配置**

在代码中通过 `MouseCursorSystem` 应用配置：

```csharp
var cursorSystem = this.GetSystem<MouseCursorSystem>();
cursorSystem.SetCursorConfig(yourConfig); // yourConfig 为你创建的 CursorConfig 实例
```

### **3. 常用 API 接口**

| 接口方法 | 说明 |
| :--- | :--- |
| `SetCursorConfig(CursorConfig config)` | 设置并应用完整的状态配置资源 |
| `SetCursorOffset(Vector2 offset)` | 设置光标相对于物理坐标的偏移（如设置中心点） |
| `ShowMouseCursor(bool show)` | 显示或隐藏自定义光标 |
| `AddMouseEnterEvent(Action action)` | 订阅鼠标进入 UI 区域事件 |
| `AddMouseExitEvent(Action action)` | 订阅鼠标离开 UI 区域事件 |
| `AddMouseDownEvent(Action action)` | 订阅鼠标左键按下事件 |
| `AddMouseMoveEvent(Action<Vector2> action)`| 订阅鼠标移动事件（参数为移动方向） |

---

## **注意事项**

- **UI 交互**: 系统通过 `EventSystem.current.IsPointerOverGameObject()` 判断是否处于 UI 上方。请确保场景中存在 `EventSystem`。
- **层级管理**: `UIMouseCursorPanel` 默认打开在 `UILevel.PopUI` 层级，以确保它显示在大多数 UI 之上。
- **性能**: 序列帧动画通过 `UISpriteAnimation` 组件播放，建议动画帧率不宜过高，以免影响性能。
- **Up 状态回退**: 当处于 `Up` 状态时，系统会等待动画播放完毕后自动根据当前位置回退到 `Hover` 或 `Default` 状态。
