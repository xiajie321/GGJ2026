using System;
using QFramework;
using Script.Service.Utility;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class EnemyController:AbsControllerBase
    {
        public override int Hp { get; set; }

        public override float Speed { get; set; }
        public override int Attack { get; set; }

        public override void Harm(HarmData data)
        {
        }

        GameConfigUility _gameConfig;
        private void Start()
        {
            Animator = GetComponent<Animator>();
            _gameConfig = this.GetUtility<GameConfigUility>();
        }
    }
}