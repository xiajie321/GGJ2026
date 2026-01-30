using QFramework;
using UnityEngine;

namespace Script.Service.View.Game.Factory
{
    public class EnemyFactory
    {
        private GameObject _prefab;
        private ResLoader _resLoader = ResLoader.Allocate();
        public void Init()
        {
            _resLoader.LoadSync<GameObject>("Enemy");
        }
    }
}