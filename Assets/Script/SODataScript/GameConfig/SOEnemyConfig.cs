using System;
using UnityEngine;

namespace Script.SODataScript.GameConfig
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "GameConfig/EnemyConfig")]
    public partial class SOEnemyConfig: AbsSOConfigBase<TbEnemyConfig>
    {
        public override TbEnemyConfig Get(int id)
        {
            TbEnemyConfig ls = _tbData[id];
            return new TbEnemyConfig()
            {
                Name = ls.Name,
                Hp = ls.Hp,
                Speed = ls.Speed,
            };
        }
    }
    [Serializable]
    public class TbEnemyConfig
    {
        public string Name ="";
        public int Hp;
        public float Speed;
    }
}