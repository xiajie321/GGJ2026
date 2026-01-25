using System;
using QFramework;
using Script.Service.Utility;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class EnemyController:AbsControllerBase
    {
        public override float Speed { get; }
        public override int Hp { get; }
        GameConfigUility _gameConfig;
        private void Start()
        {
            Animator = GetComponent<Animator>();
            _gameConfig = this.GetUtility<GameConfigUility>();
        }
    }
}