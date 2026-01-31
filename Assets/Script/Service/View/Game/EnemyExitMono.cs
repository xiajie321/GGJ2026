using System;
using QFramework;
using Script.Service.Architecture;
using Script.Service.Model;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game
{
    public class EnemyExitMono:MonoBehaviour, IController
    {
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
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}
