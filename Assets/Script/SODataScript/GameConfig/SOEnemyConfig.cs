using System;
using System.Collections.Generic;
using Alchemy.Serialization;
using UnityEngine;

namespace Script.SODataScript.GameConfig
{
    [AlchemySerialize,CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "GameConfig/EnemyConfig")]
    public partial class SOEnemyConfig:ScriptableObject
    {
        [AlchemySerializeField,NonSerialized]
        public Dictionary<int,TbEnemyConfig> TbEnemyConfigs = new();
        
    }
    [Serializable]
    public class TbEnemyConfig
    {
        public string Name;
        public int Hp;
        public float Speed;
    }
}