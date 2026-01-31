using QFramework;
using Script.Service.Architecture;
using Script.Service.System;
using Service.View.UI.Panel;
using UnityEngine;

/// <summary>
/// 不对外提供任何接口,仅作为游戏的入口
/// </summary>
public class GameBoot : MonoBehaviour,IController
{
    void Awake()
    {
        DontDestroyOnLoad(this);
        Debug.Log("[GameBoot] ResKit开始初始化...");
        ResKit.Init();
        Debug.Log("[GameBoot] ResKit初始化完成...");
        GameArchitecture.InitArchitecture();
        Debug.Log("[GameBoot] 游戏入口加载完毕");
        //UIKit.OpenPanel<UIHomePanel>();
        this.GetSystem<CameraEdgeScrollingSystem>().InitCameraSystem();
        this.GetSystem<LevelSystem>().StartLevel(0);
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
