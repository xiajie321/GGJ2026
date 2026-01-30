using System.Collections;
using System.Collections.Generic;
using QFramework;
using Script.Service.Architecture;
using Script.Service.Utility;
using UnityEngine;

public class ChoTuTestScript : MonoBehaviour,IController
{
    void Start()
    {
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
