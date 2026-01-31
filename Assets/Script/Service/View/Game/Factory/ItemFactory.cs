using QFramework;
using Script.Service.View.Game.Controller;
using UnityEngine;
using UnityEngine.Pool;

namespace Script.Service.View.Game.Factory
{
    public class ItemFactory
    {
        private GameObject _prefab;
        private ResLoader _resLoader = ResLoader.Allocate();
        private bool _isInit = false;
        private ObjectPool<GameObject> _pool;
        public void Init()
        {
            if(_isInit) return;
            _prefab = _resLoader.LoadSync<GameObject>("Item");
            _isInit = true;
            _pool = new ObjectPool<GameObject>(
                () => _prefab.Instantiate(),
                v =>
                {
                    v.SetActive(true);
                }, v =>
                {
                    v.SetActive(false);
                }, Object.Destroy,
                true,
                10,
                1000);
        }

        public void SceneChange()
        {
            _pool.Clear();
        }
         public ItemControllerMono Get(int id)
         {
             ItemControllerMono ls = _pool.Get().GetComponent<ItemControllerMono>();
             ls.transform.SetParent(null);
             ls.InitObject(id);
             return ls;
         }

        public void Release(GameObject go)
        {
            _pool.Release(go);
        }
    }
}