using System;
using System.Collections.Generic;
using Alchemy.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [AlchemySerialize]
    [ShowAlchemySerializationData]
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "ConfigUtility/LevelConfig")]
    public partial class SOLevelConfig:AbsDicScriptableObjectBase<LevelData>
    {
        public override LevelData Get(int id)
        {
            _ls = _data[id];
            return new LevelData()
            {
                Id = id,
                Name = _ls.Name,
                InitialMoney = _ls.InitialMoney,
                TrapCount = _ls.TrapCount,
                QualifiedLine = _ls.QualifiedLine,
                PerStarCost = _ls.PerStarCost,
                MaxStar = _ls.MaxStar,
                Ratio = _ls.Ratio,
                SMoveSpeed = _ls.SMoveSpeed,
                AMoveSpeed = _ls.AMoveSpeed,
                BMoveSpeed = _ls.BMoveSpeed,
                MainMenuBGM = _ls.MainMenuBGM,
                BackgroundMusic = _ls.BackgroundMusic,
                WinSound = _ls.WinSound,
                LoseSound =  _ls.LoseSound,
                TakeUpSound = _ls.TakeUpSound,
                TakeDownSound = _ls.TakeDownSound,
            };
        }
    }
    
    [Serializable]
    public class LevelData
    {
        public int Id;
        public string Name = "";
        public float InitialMoney;//场景开始时为玩家提供初始金钱
        public int TrapCount;//场景在经过随机选择陷阱后的的陷阱总数

        public float QualifiedLine;//该场景的积分的及格线
        public float PerStarCost;//每颗星所需的积分
        public int MaxStar;//该场景的最高星级
        public float Ratio;//将积分转为货币的汇率，10积分=1货币则为0.1

        public float SMoveSpeed;//遮挡等级为S的物体的移动速度
        public float AMoveSpeed;//遮挡等级为A的物体的移动速度
        public float BMoveSpeed;//遮挡等级为B的物体的移动速度

        public AudioClip ButtonSound1;//不绑定在Button上的音效1
        public AudioClip ButtonSound2;//不绑定在Button上的音效2
        
        public AudioClip MainMenuBGM;//主菜单音乐
        public AudioClip BackgroundMusic;//该关卡的背景音乐
        public AudioClip WinSound;
        public AudioClip LoseSound;
        public AudioClip TakeUpSound;//拿起家具的音效
        public AudioClip TakeDownSound;//放下家具的音效
    }
}