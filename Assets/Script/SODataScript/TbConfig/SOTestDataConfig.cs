using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewTestDataConfig", menuName = "ConfigUtility/TestDataConfig")]
    public class SOTestDataConfig:ScriptableObject
    {
        [SerializeField] private int lsData;
        public int LsData => lsData;//值类型本身就是复制所以不用像引用类型一样
        [SerializeField]
        private TestData testData;
        [SerializeField]
        private List<TestData> _testDatas;
        public List<TestData> TestDatas => new(_testDatas);//保证配置表原始数据安全必须这样做
        public TestData TestData => new()//深拷贝确保数据安全
        {
            Speed = testData.Speed,
            Damage = testData.Damage,
        };
    }
    [Serializable]
    public class TestData
    {
        public string Speed;
        public string  Damage;
    }
}