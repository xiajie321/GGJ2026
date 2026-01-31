using System.Collections;
using System.Collections.Generic;
using QFramework;
using Service.View.UI.Panel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UISelection
{
    public static void BindGlobalSelectFrame(this Button button)
    {
        var trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        RectTransform rect = button.transform as RectTransform;
        
        EventTrigger.Entry enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        enter.callback.AddListener((data) =>
        {
            // 通过 UIKit 获取全局特效面板并显示
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
