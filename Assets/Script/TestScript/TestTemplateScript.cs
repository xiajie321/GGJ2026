using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Febucci.TextAnimatorCore;
using Febucci.TextAnimatorForUnity.TextMeshPro;
using QFramework;
using Script.Service.Architecture;
using Script.Service.System;
using Script.Service.Utility;
using Service.View.UI.Panel;
using UnityEngine;

public class TestTemplateScript : MonoBehaviour,IController
{
    [SerializeField]
    private GameObject Objecta;
    void Start()
    {
        UIKit.OpenPanel<UIDamageFloatingTextPanel>();
        UIKit.OpenPanel<UICommonEffectPanel>();
        Run().Forget();
        Debug.Log(this.GetUtility<ConfigUtility>().Config.TbTestDataConfig.TestData);//通过工具拿到对应的数据
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetSystem<MessageTipSystem>().ShowTip("测试",Input.mousePosition);
        }

        if (Input.GetKey(KeyCode.W))
        {
            Objecta.transform.position += Objecta.transform.up * (Time.deltaTime * 10);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Objecta.transform.Rotate(0,0,100*Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Objecta.transform.Rotate(0,0,-100*Time.deltaTime);
        }
    }
    
    async UniTask Run()
    {
        int count = 0;
        while (true)
        {
            count++;
            this.GetSystem<DamageFloatingTextSystem>().SetText($"<sprite=0> {Random.Range(-1000,1000).ToString()}",Objecta.transform.position);//通过系统调用显示飘字
            await UniTask.Yield();
            if(count == 10) 
                return;
        }
    }
    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
