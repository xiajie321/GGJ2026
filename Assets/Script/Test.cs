using System.Collections;
using System.Collections.Generic;
using QFramework;
using Script.Service.Architecture;
using Script.Service.System;
using UnityEngine;

public class Test : MonoBehaviour,IController
{
    void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetSystem<MessageTipSystem>().ShowTip("测试",Input.mousePosition);
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
