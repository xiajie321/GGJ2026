using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "ConfigUtility/EnemyConfig")]
    public class SOEnemyConfig:AbsDicScriptableObjectBase<EnemyData>
    {
        private EnemyData _ls;
        public override EnemyData Get(int id)
        {
            _ls = _data[id];
            return new EnemyData()//深拷贝确保原始数据安全
            {
                Id = id,
                Name = _ls.Name,
                Hp = _ls.Hp,
            };
        }
    }

    public class EnemyData
    {
        public int Id;//这里可以不用填,因为在Get方法中会返回
        public string Name = "";
        public int Hp;
    }
}