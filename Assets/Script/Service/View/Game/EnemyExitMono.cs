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
using Script.Service.Utility;
using Script.SODataScript.TbConfig;

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
                    if (enemy.EnemyData.LeaveSound)
                    {
                        AudioSource.PlayClipAtPoint(enemy.EnemyData.LeaveSound, enemy.transform.position);
                    }
                    
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
            if (this.GetModel<GameModel>().EnemyControllerMonos.Count == 0)
            {
                bool hasNextLevel = SceneManager.sceneCountInBuildSettings > Index + 1;
                var levelData = this.GetUtility<ConfigUtility>().Config.TbLevelConfig.Get(Index - 1);
                int currentLevelIndex = Index;

                UIKit.OpenPanel<UIResultPanel>(UILevel.PopUI, new UIResultPanelData()
                {
                    LevelData = levelData,
                    HasNextLevel = hasNextLevel,
                    CurrentLevelIndex = currentLevelIndex
                });
            }
        }
        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}
