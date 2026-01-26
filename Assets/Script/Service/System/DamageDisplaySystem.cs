using QFramework;
using Script.Service.Architecture;
using Script.Service.Utility;
using Service.View.UI.Panel;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Script.Service.System
{
    public class DamageDisplaySystem : AbstractSystem
    {
        private SimpleObjectPool<GameObject> _pool;
        private Transform _uiRoot;
        private GameObject _damageTextPrefab;

        [SerializeField]
        public class DamageTextConfig
        {
            public float fontSize = 36f;
            public float floatSpeed = 1f;
            public float displayTime = 1f;
        }
        private DamageTextConfig _config = new DamageTextConfig();

        private void Init()
        {
            _pool = new SimpleObjectPool<GameObject>(() =>
            {
                var obj = GameObject.Instantiate(_damageTextPrefab);
                obj.SetActive(false);

                // 获取父Canvas
                _uiRoot = GameObject.Find("DamageCanvas")?.transform;
                obj.transform.SetParent(_uiRoot, false);

                return obj;
            }, initCount: 10);//对象池初始10个

            _damageTextPrefab = Resources.Load<GameObject>("Prefabs/UI/DamageText");
        }

        protected override void OnInit()
        {
            Init();
            Debug.Log("[DamageDisplaySystem] 加载完成...");
        }

        public void ShowDamage(float damageValue, Vector2 position)
        {
            var damageTextObj = _pool.Allocate();

            SetupDamageText(damageTextObj, damageValue, position);

            ActionKit.Delay(_config.displayTime, () =>
            {
                damageTextObj.SetActive(false);
                _pool.Recycle(damageTextObj);
            });
        }

        private void SetupDamageText(GameObject textObj,float damageValue, Vector2 position)
        {
            textObj.SetActive(true);
            textObj.transform.position = position;

            var textMesh = textObj.GetComponent<TextMeshProUGUI>();
            textMesh.text = damageValue.ToString();
        }
    }
}