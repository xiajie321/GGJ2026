using System;
using QFramework;
using Script.Service.Utility;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class PlayerController:AbsControllerBase
    {
        public override int Hp { get; set; }

        public override float Speed { get; set; }

        public override void Harm(HarmData data)
        {
        }
        private int _speed;
        private int _hp;
        private Rigidbody2D _rigidbody2D;
        [SerializeField]
        private GameObject attackGameObject;//用于鼠标控制attack的游戏对象的开关。
        GameConfigUility _gameConfig;
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            _gameConfig = this.GetUtility<GameConfigUility>();
            //TODO 使用Qf的状态机实现移动、跳跃、攻击、受伤后的一段时间的无敌状态的切换
            // 移动：左右（AD）：左右移动不影响角色朝向，角色始终面向玩家鼠标
            // 跳（W、空格）：向上跳跃，略高于僵尸。
            // 攻击（左键）:前方小范围（跳跃高度能通过配置表拿到）扇形攻击，击杀所有攻击框内的僵尸
            // 血量：碰撞即扣血，闪烁变红，同时给个1s的无敌（不断闪烁，类似双箭头）
        }

        public void Update()
        {
            if (Input.GetKey(_gameConfig.GameConfig.PlayerConfig.MoveLeftKey))//左移动的绑定
            {
                
            }

            if (Input.GetKey(_gameConfig.GameConfig.PlayerConfig.MoveRightKey))//右移动的绑定
            {
                
            }
            if (Input.GetKeyDown(_gameConfig.GameConfig.PlayerConfig.JumpKey))//跳跃的绑定
            {
                
            }
            if (Input.GetKeyDown(_gameConfig.GameConfig.PlayerConfig.AttackKey))//攻击的绑定
            {
                
            }
        }
    }
}