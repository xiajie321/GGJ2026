using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamageTextComponent : MonoBehaviour
{
    private TextMeshProUGUI _text;
    public TextMeshProUGUI TextComponent => _text ??= GetComponent<TextMeshProUGUI>();
    
    public void SetText(string text)
    {
        TextComponent.text = text;
    }
    public void SetColor(Color color)
    {
        TextComponent.color = color;
    }
}
