using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Service.View.Game.Controller.SceneController
{
    public class ClearAreaMono:MonoBehaviour
    {
        HashSet<Collider2D> values = new();

        private void OnTriggerEnter2D(Collider2D other)
        {
            values.Add(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            values.Remove(other);
        }
        /// <summary>
        /// 清除碎块
        /// </summary>
        /// <returns></returns>
        public int ClearArea()
        {
            int count = values.Count;
            foreach (var value in values)
            {
                Destroy(value.gameObject);
            }
            return count;
        }
    }
}