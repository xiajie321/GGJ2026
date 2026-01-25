using UnityEditor.Animations;
using UnityEngine;

namespace Script.SODataScript.GameConfig
{
    [CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "GameConfig/PlayerConfig")]
    public class SOPlayerConfig:ScriptableObject
    {
        public KeyCode JumpKey;
        public KeyCode MoveLeftKey;
        public KeyCode MoveRightKey;
        public KeyCode AttackKey;
        public AnimatorController AnimatorController;
        public int Hp;
        public int Speed;
        public float JumpHeight = 1.5f;
        public int Attack;
        public float InvincibilityTime = 1f;//无敌时间
    }
}