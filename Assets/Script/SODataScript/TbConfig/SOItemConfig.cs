using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.SODataScript.TbConfig
{
    [CreateAssetMenu(fileName = "NewItemConfig", menuName = "ConfigUtility/ItemConfig")]
    public class SOItemConfig:AbsDicScriptableObjectBase<ItemData>
    {
        public override ItemData Get(int id)
        {
            _ls = _data[id];
            return new ItemData()
            {
                Id = id,
                Name = _ls.Name,
                Sprite = _ls.Sprite,
                Type = _ls.Type,
                Height = _ls.Height,
                Level = _ls.Level,
                IsMoveable = _ls.IsMoveable,
            };
        }
    }
    [Serializable]
    public class ItemData
    {
        public int Id;//这里可以不用填,因为在Get方法中会返回
        public string Name = "";
        public Sprite Sprite;

        public ItemType Type;//物品类型

        public Height Height;//物品的高度
        public ItemLevel Level;//遮挡等级
        
        public bool IsMoveable;//物品是否可以移动
        
    }
    
    /// <summary>
    /// 物品所需的类型
    /// 目前只有一个Type类型,后续可以扩展
    /// </summary>
    [Serializable]
    public enum ItemType
    {
        Type = 0,
    }
    /// <summary>
    /// 物品能遮挡住的物体的等级
    /// 高档物品一定能遮挡住低档漏洞
    /// 低档物品一定不能遮挡住高档漏洞
    /// </summary>
    [Serializable]
    public enum ItemLevel
    {
        S = 0,
        A = 1,
        B = 2,
        F = 3,  //仅装饰用，无法挡住任何漏洞
    }

    /// <summary>
    /// 物品以及漏洞的高度
    /// 只有高度匹配的物品才能够吸附在对应高度的漏洞上
    /// </summary>
    [Serializable]
    public enum Height
    {
        Wall = 0,//墙面
        WallRoot = 1,//墙根
        Ground = 2,//地面
    }
}