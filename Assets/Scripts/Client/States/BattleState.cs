using System;
using UnityEngine;

namespace Client.States
{
    public abstract class BattleState
    {
        protected BattleStateMachine Owner { get; private set; }
        public abstract EBattleStateId Id { get; }
        public event Action OnEnter;
        public event Action OnExit;
        public event Action<EBattleStateId> OnComplete;

        public void Setup(BattleStateMachine owner)
        {
            Owner = owner;
        }

        public void Enter()
        {
            Subscribe();
            EnterInner();
            Debug.Log($"Entering Battle State {Id}");
            OnEnter?.Invoke();
        }

        public void Exit()
        {
            Unsubscribe();
            ExitInner();
            Debug.Log($"Exiting Battle State {Id}");
            OnExit?.Invoke();
        }

        protected virtual void EnterInner()
        {
        }

        protected virtual void ExitInner()
        {
        }

        protected virtual void Subscribe()
        {
            
        }

        protected virtual void Unsubscribe()
        {
            
        }

        protected void Complete()
        {
            OnComplete?.Invoke(Id);
        }
    }
}