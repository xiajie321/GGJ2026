# MessageTipSystem (消息提示系统) 使用说明文档

`MessageTipSystem` 是一个基于 QFramework 架构的消息提示系统，用于在游戏中显示简短的提示信息或带有标题的消息弹窗。

## 系统架构

系统由以下部分组成：

- **MessageTipSystem.cs**: 系统入口，提供显示提示和消息的接口。
- **UIMessageTipPanel**: UI 表现层，负责消息的实际渲染和动画显示。

---

## 核心功能

1. **简短提示 (Tip)**: 在屏幕上显示一段会自动消失的文字。
2. **消息弹窗 (Message)**: 显示带有标题和正文的消息窗口。
3. **坐标定位**: 支持在指定屏幕位置显示提示或消息。

---

## 使用指南

### 1. 获取系统

在 QFramework 架构中通过以下方式获取系统：

```csharp
var messageTipSystem = this.GetSystem<MessageTipSystem>();
```

### 2. 显示提示信息

```csharp
// 在默认位置显示提示
messageTipSystem.ShowTip("操作成功");

// 在指定位置显示提示
messageTipSystem.ShowTip("发现物品", new Vector2(500, 500));
```

### 3. 显示消息弹窗

```csharp
// 显示标准消息弹窗
messageTipSystem.ShowMessage("提示", "您确定要执行此操作吗？");

// 在指定位置显示消息弹窗
messageTipSystem.ShowMessage("警告", "系统资源不足", new Vector2(Screen.width / 2, Screen.height / 2));
```

---

## API 接口说明

| 接口方法 | 说明 |
| :--- | :--- |
| `ShowTip(string message)` | 在默认位置显示简短提示 |
| `ShowTip(string message, Vector2 position)` | 在指定屏幕位置显示简短提示 |
| `ShowMessage(string title, string message)` | 显示带有标题的消息弹窗 |
| `ShowMessage(string title, string message, Vector2 position)` | 在指定屏幕位置显示带有标题的消息弹窗 |

---

## 注意事项

- **UI 层级**: `UIMessageTipPanel` 默认在 `UILevel.PopUI` 层级打开，确保其显示在大多数 UI 之上。
- **自动初始化**: 系统在 `OnInit` 时会自动打开 `UIMessageTipPanel`，无需手动干预。
