using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "ConfigUtility/EnemyConfig")]
    public class SOEnemyConfig:AbsDicScriptableObjectBase<EnemyData>
    {
        public override EnemyData Get(int id)
        {
            _ls = _data[id];
            return new EnemyData()//深拷贝确保原始数据安全
            {
                Id = id,
                Name = _ls.Name,
                RuntimeAnimatorController = _ls.RuntimeAnimatorController,
                MoveSpeed = _ls.MoveSpeed,
                LoveType = _ls.LoveType,
                Reword = _ls.Reword,
                StandTime = _ls.StandTime,
                ThinkTime = _ls.ThinkTime,
            };
        }
    }

    public class EnemyData
    {
        public int Id;//这里可以不用填,因为在Get方法中会返回
        public string Name = "";
        public RuntimeAnimatorController RuntimeAnimatorController;//动画控制器替换
        public float MoveSpeed;//移动速度
        public ItemType LoveType;//喜欢的物品类型
        public float Reword;//发现喜爱的物品时获得的奖励
        public float StandTime;//站立什么都不做的时间
        public float ThinkTime;//对物品或陷阱进行思考的时间
    }
    
}