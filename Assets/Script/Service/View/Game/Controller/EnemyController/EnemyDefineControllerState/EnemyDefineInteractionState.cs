using QFramework;
using Script.Service.Architecture;
using Script.Service.System;
using UnityEngine;

namespace Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState
{
    public class EnemyDefineInteractionState:AbstractState<EnemyState,EnemyDefineControllerData>,IController
    {
        public EnemyDefineInteractionState(FSM<EnemyState> fsm, EnemyDefineControllerData owner) : base(fsm, owner)
        {
            
        }
        protected override void OnEnter()
        {
            // 概率判定
            int randomVal = Random.Range(0, 100);
            if (randomVal >= mOwner.EnemyData.InteractPBTY)
            {
                Debug.Log($"[敌人交互] 跳过交互。随机值: {randomVal}, 概率: {mOwner.EnemyData.InteractPBTY}");
                mFSM.ChangeState(EnemyState.Move);
                return;
            }
            Debug.Log($"[敌人交互] 进入交互。随机值: {randomVal}, 概率: {mOwner.EnemyData.InteractPBTY}");
            
            mOwner.Animator.Play("Interact");
            _time = 0;
            _thinking = false;
            _ls = null;
        }
        bool _thinking = false;
        float _time = 0;
        private TrapControllerMono _ls;
        protected override void OnUpdate()
        {
            if (mOwner.EnemyTriggerMono.TrapControllerMonos.Count == 0)
            {
                mFSM.ChangeState(EnemyState.Move);
                _thinking = false;
                return;
            }

            if (_thinking)
            {
                _time += UnityEngine.Time.deltaTime;
            }

            if (_time >= mOwner.EnemyData.InteractTime)
            {
                if (_ls == null || _ls.TrapAdsorberMono == null)
                {
                    Debug.LogWarning("[敌人交互] 陷阱为空或缺少组件。中止交互。");
                    _thinking = false;
                    mFSM.ChangeState(EnemyState.Move);
                    return;
                }

                if (_ls.TrapAdsorberMono.IsJudgment)
                {
                    Debug.Log(_ls.TrapAdsorberMono.ItemControllerMono.ItemData);
                    if (_ls.TrapAdsorberMono.ItemControllerMono.ItemData.Type == mOwner.EnemyData.LoveType)
                    {
                        //this.GetSystem<DamageFloatingTextSystem>().SetText($"{0}",mOwner.Rigidbody2D.transform.position);//心情增加
                    }
                }
                else
                {
                    // 心情损耗
                    mOwner.CurrentMood = Mathf.Max(0, mOwner.CurrentMood - mOwner.EnemyData.LostOfMood);
                    mOwner.TotalLostMood += mOwner.EnemyData.LostOfMood;
                    this.GetSystem<DamageFloatingTextSystem>().SetText($"-{mOwner.EnemyData.LostOfMood}",Color.red,mOwner.Rigidbody2D.transform.position);//心情损耗
                    Debug.Log($"[敌人交互] 心情下降。当前心情: {mOwner.CurrentMood}, 累计损失: {mOwner.TotalLostMood}");
                    EnemyDefineController.UpdateEmoji(mOwner);
                    
                    // 破防逻辑
                    if (mOwner.CurrentMood <= 0)
                    {
                        mOwner.IsAngry = true;
                        Debug.Log($"[敌人交互] 敌人破防了！(生气状态)");
                    }
                }
                _thinking = false;
                mFSM.ChangeState(EnemyState.Move);
                return;
            }
            if(_thinking) return;
            _ls = mOwner.EnemyTriggerMono.TrapControllerMonos[^1];
            _thinking =  true;
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}
