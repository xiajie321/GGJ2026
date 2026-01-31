using System;
using System.Collections.Generic;
using Script.Service.View.Game.Controller;
using UnityEngine;

namespace Script.Service.View.Game
{
    public class EnemyTriggerMono:MonoBehaviour
    {
        private readonly List<TrapControllerMono> _trapControllerMonos = new();
        private readonly List<ItemControllerMono> _itemControllers = new();
        public List<TrapControllerMono> TrapControllerMonos => _trapControllerMonos;
        public List<ItemControllerMono> ItemControllers => _itemControllers;
        private void OnTriggerEnter2D(Collider2D other)
        {
            var ls = other.GetComponent<TrapControllerMono>();
            _trapControllerMonos.Add(ls);
            if(ls.TrapAdsorberMono.IsJudgment)
                _itemControllers.Add(other.transform.GetChild(0).GetComponent<ItemControllerMono>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var ls = other.GetComponent<TrapControllerMono>();
            _trapControllerMonos.Remove(ls);
            var ls2 = other.transform?.GetChild(0)?.GetComponent<ItemControllerMono>();
            if(_itemControllers.Contains(ls2))
                _itemControllers.Remove(ls2);
        }
    }
}