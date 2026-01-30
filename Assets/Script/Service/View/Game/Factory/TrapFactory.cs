using QFramework;
using UnityEngine;
using UnityEngine.Pool;

namespace Script.Service.View.Game.Factory
{
    public class TrapFactory
    {
        private GameObject _prefab;
        private ResLoader _resLoader = ResLoader.Allocate();
        private bool _isInit = false;
        private ObjectPool<GameObject> _pool;
        public void Init()
        {
            if(_isInit) return;
            _prefab = _resLoader.LoadSync<GameObject>("Trap");
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
         public TrapControllerMono Get(int id)
         {
             TrapControllerMono ls = _pool.Get().GetComponent<TrapControllerMono>();
             ls.InitObject(id);
             return ls;
         }

        public void Release(GameObject go)
        {
            _pool.Release(go);
        }
    }
}