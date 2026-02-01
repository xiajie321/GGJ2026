using System;
using Cysharp.Threading.Tasks;
using QFramework;
using Script.Service.Architecture;
using Script.Service.Model;
using Script.Service.System;
using Script.Service.View.Game.Controller;
using Script.Service.View.UI.Panel;
using Service.View.UI.Panel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Service.View.Game
{
    public class EnemyExitMono:MonoBehaviour, IController
    {
        public static int Index = 1;
        private void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.GetComponent<EnemyControllerMono>();
            if (enemy != null)
            {
                var controller = enemy.Controller;
                if (controller != null && controller.Data != null)
                {
                    if (controller.Data.CurrentMood > 0)
                    {
                        float tip = controller.Data.CurrentMood * enemy.EnemyData.MoodBoot;
                        Debug.Log($"[敌人离场] 增加小费: {tip}。心情: {controller.Data.CurrentMood}, 倍率: {enemy.EnemyData.MoodBoot}");
                        this.GetModel<LevelModel>().AddMoney(tip, MoneySource.LevelNpc);
                    }
                    else
                    {
                        Debug.Log($"[敌人离场] 无小费。心情: {controller.Data.CurrentMood}");
                    }
                }
            }
            other.gameObject.SetActive(false);
            if (this.GetModel<GameModel>().EnemyControllerMonos.Count == 0 && SceneManager.sceneCountInBuildSettings > Index)
            {
                Index++;
                this.GetSystem<SceneSwitchSystem>().LoadSceneAsync<UILoadingPanel>($"Level{Index}", v =>
                {
                    Run(v).Forget();
                    
                });
            }
            else if(this.GetModel<GameModel>().EnemyControllerMonos.Count == 0)
            {
                this.GetSystem<SceneSwitchSystem>().LoadSceneAsync<UILoadingPanel>("GameBoot", v =>
                {
                    UIKit.CloseAllPanel();
                    UIKit.OpenPanel<UIHomePanel>();
                    UIKit.ClosePanel(v);
                });
            }
        }
        private async UniTask Run(UIPanel panel)
        {
            await UniTask.Delay(500);
            this.GetSystem<LevelSystem>().StartLevel(Index); // 默认关卡
            await UniTask.Yield();
            UIKit.CloseAllPanel();
            UIKit.OpenPanel<UIMouseCursorPanel>(UILevel.PopUI);
            UIKit.OpenPanel<UIHUDPanel>();
			await UniTask.Yield();
            UIKit.ClosePanel(panel);
        }
        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}
