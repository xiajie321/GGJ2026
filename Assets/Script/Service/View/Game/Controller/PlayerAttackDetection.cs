using System;
using UnityEngine;

namespace Script.Service.View.Game.Controller
{
    public class PlayerAttackDetection:MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            //图层筛选已经在编辑器搞好了
            other.GetComponent<EnemyController>();
        }
    }
}