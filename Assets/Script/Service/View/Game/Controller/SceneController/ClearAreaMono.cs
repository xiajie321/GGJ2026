using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;

namespace Script.Service.View.Game.Controller.SceneController
{
    public class ClearAreaMono : MonoBehaviour
    {
        readonly HashSet<Collider2D> values = new();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            values.Add(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            values.Remove(other);
        }

        public HashSet<Collider2D> GetValues()
        {
            return values;
        }
        /// <summary>
        /// 清除碎块
        /// </summary>
        /// <returns></returns>
        public int ClearArea()
        {
            int count = values.Count;
            // 创建一个临时列表来存储所有要销毁的Collider2D
            List<Collider2D> toDestroy = new List<Collider2D>(values);

            foreach (var value in toDestroy)
            {
                Destroy(value.gameObject);
            }

            values.Clear();
            return count;
        }
    }
}