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
                
                MoodBoot = _ls.MoodBoot,
                MaxMood = _ls.MaxMood,
                LostOfMood = _ls.LostOfMood,
                MoodHp1 = _ls.MoodHp1,
                MoodHp2 = _ls.MoodHp2,
                ThinkProbability = _ls.ThinkProbability,
                
                StandTime = _ls.StandTime,
                ThinkTime = _ls.ThinkTime,
                ThinkCoolingTime = _ls.ThinkCoolingTime,
                
                InteractPBTY = _ls.InteractPBTY,
                InteractTime = _ls.InteractTime,
                
                EnterSound = _ls.EnterSound,
                LeaveSound = _ls.LeaveSound,
                InteractSound = _ls.InteractSound,
                PassSound = _ls.PassSound,
                ExposedSound = _ls.ExposedSound,
                YellowFaceSound = _ls.YellowFaceSound,
                RedFaceSound = _ls.RedFaceSound,
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

        public float MoodBoot;//Enemy退出场景时根据剩余心情获取金币，<MoodBoot>金币per心情
        public int MaxMood;//心情值上限
        public int LostOfMood;//每看到一个漏洞扣除的心情
        public int MoodHp1;//该NPC累计损失心情达到该值时，心情状态从开心变为思考
        public int MoodHp2;//该NPC累计损失心情达到该值时，心情状态从思考变为生气
        public int ThinkProbability;//NPC看到漏洞后进入思考状态的概率，用100~0表示百分比
        
        public float StandTime;//站立什么都不做的时间（秒）
        public float ThinkTime;//对物品或陷阱进行思考的时间（秒）
        public float ThinkCoolingTime;
        
        public int InteractPBTY; // 交互概率, 1-100
        public float InteractTime; // 交互时间

        public AudioClip EnterSound;//敌人生成时的音效
        public AudioClip LeaveSound;//敌人离开（销毁）时
        public AudioClip InteractSound;//开始思考过程的音效
        public AudioClip PassSound;//检查通过的音效
        public AudioClip ExposedSound;//发现破绽的音效
        public AudioClip YellowFaceSound;//切换到黄脸时的音效
        public AudioClip RedFaceSound;//切换到红脸时的音效
        
    }
    
}
