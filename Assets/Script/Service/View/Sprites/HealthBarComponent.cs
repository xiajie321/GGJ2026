using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarComponent : MonoBehaviour
{
    private SpriteRenderer _bg;
    private SpriteRenderer _fill;
    public SpriteRenderer Bg => transform.Find("Bg").GetComponent<SpriteRenderer>();
    public SpriteRenderer Fill => transform.Find("Fill").GetComponent<SpriteRenderer>();

    public void SetHealth(float current, float max)
    {
        Vector3 scale = _fill.transform.localScale;
        scale.x = current / max;
        Fill.transform.localScale = scale;
    }

    public void SetColor(Color fillColor, Color bgColor)
    {
        Fill.color = fillColor;
        Bg.color = bgColor;
    }
}
