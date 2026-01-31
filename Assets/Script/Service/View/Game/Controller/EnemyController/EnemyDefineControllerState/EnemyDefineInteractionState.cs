using QFramework;
using Script.Service.Architecture;
using Script.Service.System;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Service.View.Game.Controller.EnemyController.EnemyDefineControllerState
{
    public class EnemyDefineInteractionState:AbstractState<EnemyState,EnemyDefineControllerData>,IController
    {
        public EnemyDefineInteractionState(FSM<EnemyState> fsm, EnemyDefineControllerData owner) : base(fsm, owner)
        {
            
        }
        protected override void OnEnter()
        {
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

            if (_time >= mOwner.EnemyData.ThinkTime)
            {
                if (_ls.TrapAdsorberMono.IsJudgment)
                {
                    Debug.Log(_ls.TrapAdsorberMono.ItemControllerMono.ItemData);
                    if (_ls.TrapAdsorberMono.ItemControllerMono.ItemData.Type == mOwner.EnemyData.LoveType)
                    {
                        //TODO 这里加分
                        Debug.Log(mOwner.EnemyData.Reward);
                        this.GetSystem<DamageFloatingTextSystem>().SetText($"{mOwner.EnemyData.Reward}",mOwner.Rigidbody2D.transform.position);
                    }
                }
                else
                {
                    //TODO 这里扣分
                    Debug.Log($"{-mOwner.EnemyData.Reward}");
                    this.GetSystem<DamageFloatingTextSystem>().SetText($"{-mOwner.EnemyData.Reward}",mOwner.Rigidbody2D.transform.position);
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