using System.Collections;
using System.Collections.Generic;
using QFramework;
using Service.View.UI.Panel;
using UnityEngine;

public class OpenMainPanel : MonoBehaviour
{
    void Start()
    {
        UIKit.OpenPanel<UIHomePanel>();
    }
}
