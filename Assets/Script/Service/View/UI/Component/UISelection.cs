using QFramework;
using Service.View.UI.Panel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UISelection
{
    class UISelectionCleaner : MonoBehaviour
    {
        private void OnDisable()
        {
            // 当按钮被隐藏、销毁、或场景切换导致物体失效时触发
            // 只有当面板存在时才调用
            var panel = UIKit.GetPanel<UICommonEffectPanel>();
            if (panel != null)
            {
                panel.HideFrame();
            }
        }
    }

    public static void BindGlobalSelectFrame(this Button button)
    {
        // 检查是否已经添加过，防止重复添加
        if (button.gameObject.GetComponent<UISelectionCleaner>() == null)
        {
            button.gameObject.AddComponent<UISelectionCleaner>();
        }

        var trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        RectTransform rect = button.transform as RectTransform;

        EventTrigger.Entry enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        enter.callback.AddListener((data) =>
        {
            var panel = UIKit.GetPanel<UICommonEffectPanel>();
            if (panel != null)
            {
                panel.ShowFrame(button.transform.position, rect.sizeDelta);
            }
        });
        trigger.triggers.Add(enter);
        
        EventTrigger.Entry exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exit.callback.AddListener((data) =>
        {
            UIKit.GetPanel<UICommonEffectPanel>()?.HideFrame();
        });
        trigger.triggers.Add(exit);
    }
}