using System.Collections;
using System.Collections.Generic;
using QFramework;
using Script.Service.Utility;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MessageTipUtility.Instance.ShowTip("测试",Input.mousePosition);
        }
    }
}
