using System;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class EnemyControllerMono:AbsControllerMono<EnemyState>
    {
        public Animator Animation => _animation;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Collider2D Collider2D => _collider2D;
        private void Start()
        {
            InitComponents();
        }
        private void Update()
        {
            _fsm.Update();
        }
        private void FixedUpdate()
        {
            _fsm.FixedUpdate();
        }
        private void OnGUI()
        {
            _fsm.OnGUI();
        }
    }
}