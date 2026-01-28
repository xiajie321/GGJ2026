using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarComponent : MonoBehaviour
{
    private Slider _slider;
    public Slider HealthBarComponent => _slider ??= GetComponent<Slider>();

    public void SetHealth(float current, float max)
    {
        HealthBarComponent.value = current;
        HealthBarComponent.maxValue = max;
    }

    private Image _fillImage;
    private Image _bgImage;
    public void SetColor(Color fillColor, Color bgColor)
    {
        _fillImage = HealthBarComponent.fillRect?.GetComponent<Image>();
        _fillImage.color = fillColor;
        _bgImage = HealthBarComponent.transform.Find("Bg").GetComponent<Image>();
        _bgImage.color = bgColor;
    }
}
