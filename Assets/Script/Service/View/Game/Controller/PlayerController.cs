using System;
using QFramework;
using Script.Service.Model;
using Script.Service.Utility;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class PlayerController:AbsControllerBase
    {
        public override float Speed { get; }
        public override int Hp { get; }
        private int _speed;
        private int _hp;
        private Rigidbody2D _rigidbody2D;
        GameConfigUility _gameConfig;
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            _gameConfig = this.GetUtility<GameConfigUility>();
            //TODO 使用Qf的状态机实现移动、跳跃、攻击、受伤后的一段时间的无敌状态的切换
        }

        public void Update()
        {
            if (Input.GetKey(_gameConfig.GameConfig.PlayerConfig.MoveLeftKey))//左移动的绑定
            {
                
            }

            if (Input.GetKey(_gameConfig.GameConfig.PlayerConfig.MoveRightKey))//右移动的绑定
            {
                
            }
            if (Input.GetKey(_gameConfig.GameConfig.PlayerConfig.JumpKey))//跳跃的绑定
            {
                
            }
            if (Input.GetKey(_gameConfig.GameConfig.PlayerConfig.AttackKey))//攻击的绑定
            {
                
            }
        }
    }
}