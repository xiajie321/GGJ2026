using UnityEngine;

namespace Script.SODataScript.GameConfig
{
    [CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "GameConfig/PlayerConfig")]
    public class SOPlayerConfig:ScriptableObject
    {
        public KeyCode Jump;
        public KeyCode MoveLeft;
        public KeyCode MoveRight;
        public KeyCode Attack;
        public int Hp;
        public int Speed;
        public int JumpHeight;
    }
}