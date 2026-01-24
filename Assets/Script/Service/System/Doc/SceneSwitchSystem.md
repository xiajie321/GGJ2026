# SceneSwitchSystem (场景切换系统) 使用说明文档

`SceneSwitchSystem` 是一个基于 QFramework 架构的异步场景管理系统。它支持在切换场景时通过自定义 UI 面板展示加载进度，并提供了灵活的加载控制逻辑。

## 系统架构

系统由以下核心部分组成：

- **SceneSwitchSystem.cs**: 系统主入口，负责管理场景的异步加载和卸载过程。
- **ISceneSwitch (接口)**: 所有的加载 UI 面板必须实现此接口，以便系统能够传递加载状态。

---

## ISceneSwitch 接口定义

为了配合 `SceneSwitchSystem` 工作，自定义的加载 UI 面板需要实现以下接口：

```csharp
public interface ISceneSwitch
{
    /// <summary>
    /// 正在 (加载/卸载) 场景时会调用的方法。
    /// </summary>
    /// <param name="progress">加载进度 (0 到 1)</param>
    /// <param name="isLoad">true 为加载场景，false 为卸载场景</param>
    void OnLoad(float progress, bool isLoad);

    /// <summary>
    /// (加载/卸载) 场景进度达到 0.9 以上时调用。
    /// </summary>
    /// <param name="progress">当前进度</param>
    /// <param name="isLoad">true 为加载场景，false 为卸载场景</param>
    /// <returns>返回 true 则立即激活场景，返回 false 则保持当前状态（可用于等待用户点击等）</returns>
    bool OnLoadCompleted(float progress, bool isLoad);
}
```

---

## 核心功能

1. **异步场景加载**: 支持 `Single` 或 `Additive` 模式加载场景。
2. **自定义加载界面**: 开发者可以为不同的场景切换过程指定不同的加载 UI。
3. **加载进度回调**: 通过 `OnLoad` 方法实时获取进度。
4. **加载完成控制**: 通过 `OnLoadCompleted` 返回值控制场景激活时机。

---

## 使用指南

### 1. 准备加载 UI

创建一个 UIPanel 并实现 `ISceneSwitch` 接口：

```csharp
public class UILoadingPanel : UIPanel, ISceneSwitch
{
    public void OnLoad(float progress, bool isLoad)
    {
        // 更新进度条 UI
        progressBar.fillAmount = progress;
    }

    public bool OnLoadCompleted(float progress, bool isLoad)
    {
        // 可以在这里等待动画结束或用户点击后再返回 true
        return true; 
    }
}
```

### 2. 调用场景加载

```csharp
var sceneSystem = this.GetSystem<SceneSwitchSystem>();

// 异步加载名为 "GameScene" 的场景，并使用 UILoadingPanel 作为加载界面
sceneSystem.LoadSceneAsync<UILoadingPanel>("GameScene");
```

### 3. 调用场景卸载

```csharp
// 异步卸载名为 "Level1" 的场景
sceneSystem.UnloadSceneAsync<UILoadingPanel>("Level1");
```

---

## API 接口说明

| 接口方法 | 说明 |
| :--- | :--- |
| `LoadSceneAsync<TUIPanel>(string sceneName, LoadSceneMode mode)` | 异步加载场景。`TUIPanel` 必须实现 `ISceneSwitch` |
| `UnloadSceneAsync<TUIPanel>(string sceneName)` | 异步卸载场景。`TUIPanel` 必须实现 `ISceneSwitch` |

---

## 注意事项

- **单例 UI**: 系统会管理加载 UI 的生命周期。如果新请求的 UI 类型与当前已打开的相同，系统会复用该 UI。
- **进度限制**: 由于 Unity 的 `AsyncOperation.progress` 在 `allowSceneActivation = false` 时最高只能达到 0.9，因此系统在进度达到 0.9 时就会触发 `OnLoadCompleted`。
- **依赖项**: 本系统依赖 `UniTask` 进行异步处理。
