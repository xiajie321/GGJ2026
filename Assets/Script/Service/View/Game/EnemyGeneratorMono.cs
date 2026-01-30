using System;
using QFramework;
using Script.Service.Architecture;
using Script.Service.Command;
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
            this.SendCommand(new SetEnemyGeneratorMonoCommand(this));
        }

        public float GetMaxTimeLength()//获取关卡最大时间长度
        {
            if (_enemyGeneratorConfig.Enemys.Count == 0)
            {
                return 0;
            }
            return _enemyGeneratorConfig.Enemys[^1].UpdateTime;
        }

        public float GetCurrentTimeLength()
        {
            return _time;
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
                for (int i = 0; i < _enemyGeneratorConfig.Enemys[_index].Data.Count; i++)
                {
                    for (int j = 0; j < _enemyGeneratorConfig.Enemys[_index].Data[i].Sum; j++)
                    {
                        _factory.Get(_enemyGeneratorConfig.Enemys[_index].Data[i].Id);
                    }
                }
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}