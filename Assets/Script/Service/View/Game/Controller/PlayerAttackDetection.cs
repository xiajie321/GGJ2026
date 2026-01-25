using System;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class PlayerAttackDetection:MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController;
        private void OnTriggerEnter2D(Collider2D other)
        {
            //图层筛选已经在编辑器搞好了
            other.GetComponent<EnemyController>().Harm(new()
            {
                Hp = _playerController.Attack,
            });
        }
    }
}