using UnityEditor.Animations;
using UnityEngine;

namespace Script.SODataScript.GameConfig
{
    [CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "GameConfig/PlayerConfig")]
    public class SOPlayerConfig:ScriptableObject
    {
        public KeyCode JumpKey = KeyCode.W;
        public KeyCode MoveLeftKey = KeyCode.A;
        public KeyCode MoveRightKey =  KeyCode.D;
        public KeyCode AttackKey  = KeyCode.Space;
        public RuntimeAnimatorController AnimatorController;
        public int MaxHp;
        public float MaxSpeed;
        public int Hp;
        public float Speed;
        public float JumpHeight = 1.5f;
        public int Attack;
        public float InvincibilityTime = 1f;//无敌时间
    }
}