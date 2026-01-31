using System;
using System.Collections.Generic;
using QFramework;
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
            if (ls != null)
            {
                _trapControllerMonos.Add(ls);
                if (ls.TrapAdsorberMono != null && ls.TrapAdsorberMono.IsJudgment)
                {
                    if (other.transform.childCount > 0)
                    {
                        var item = other.transform.GetChild(0).GetComponent<ItemControllerMono>();
                        if (item != null) _itemControllers.Add(item);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var ls = other.GetComponent<TrapControllerMono>();
            if (ls != null)
            {
                _trapControllerMonos.Remove(ls);
            }

            if (other.transform.childCount > 0)
            {
                var ls2 = other.transform.GetChild(0).GetComponent<ItemControllerMono>();
                if (ls2 != null && _itemControllers.Contains(ls2))
                {
                    _itemControllers.Remove(ls2);
                }
            }
        }
    }
}