using System;
using QFramework;
using Script.Service.Architecture;
using Script.Service.System;
using Script.Service.Utility;
using Script.Service.View.Game.Factory;
using Script.SODataScript.TbConfig;
using UnityEngine;

namespace Script.Service.View.Game
{
    public class EnemyGeneratorMono:MonoBehaviour,IController
    {
        [SerializeField] private int GeneratorId;
        private EnemyGeneratorConfig _enemyGeneratorConfig;
        private EnemyFactory _factory;
        private void Start()
        {
            _enemyGeneratorConfig = this.GetUtility<ConfigUtility>().Config.TbEnemyGeneratorConfig.Get(GeneratorId);
            _factory = this.GetSystem<FactorySystem>().EnemyFactory;
        }

        private float _time;
        private int _index = 0;
        private void Update()
        {
            _time += Time.deltaTime;
            if (_enemyGeneratorConfig.Enemys.Count == 0)
            {
                return;
            }

            if (_enemyGeneratorConfig.Enemys[_index].UpdateTime >= _time)
            {
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}