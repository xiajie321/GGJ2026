using System.Collections.Generic;
using Alchemy.Serialization;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [AlchemySerialize]
    [ShowAlchemySerializationData]
    [CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "ConfigUtility/EnemyConfig")]
    public partial class SOEnemyConfig:AbsDicScriptableObjectBase<EnemyData>
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
                TicketCost = _ls.TicketCost,
                Reward = _ls.Reward,
                DeductPoints = _ls.DeductPoints,
                StandTime = _ls.StandTime,
                ThinkTime = _ls.ThinkTime,
                ThinkCoolingTime = _ls.ThinkCoolingTime,
            };
        }
    }

    [System.Serializable]
    public class EnemyData
    {
        public int Id;//这里可以不用填,因为在Get方法中会返回
        public string Name = "";
        public RuntimeAnimatorController RuntimeAnimatorController;//动画控制器替换
        public float MoveSpeed;//移动速度
        public ItemType LoveType;//喜欢的物品类型
        public float TicketCost;//进场时玩家获得的门票钱
        public float Reward;//发现喜爱的物品时获得的奖励
        public float DeductPoints;//被发现破洞时扣除的分数
        public float StandTime;//站立什么都不做的时间（秒）
        public float ThinkTime;//对物品或陷阱进行思考的时间（秒）
        public float ThinkCoolingTime;
    }
    
}