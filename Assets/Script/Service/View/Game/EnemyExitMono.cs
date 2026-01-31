using System;
using UnityEngine;

namespace Script.Service.View.Game
{
    public class EnemyExitMono:MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.SetActive(false);
        }
    }
}