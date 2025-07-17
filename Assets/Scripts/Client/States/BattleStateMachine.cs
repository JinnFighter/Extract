using System;
using System.Collections.Generic;
using Common;
using Logic;
using UnityEngine;
using UnityEngine.Events;

namespace Client.States
{
    public class BattleStateMachine
    {
        private readonly Dictionary<EBattleStateId, BattleState> _states = new()
        {
            { EBattleStateId.Init, new BattleStateInit() },
            { EBattleStateId.PlayerTurn, new BattleStatePlayerTurn() },
            { EBattleStateId.ActionSelectTile, new BattleStateActionSelectTile()},
            { EBattleStateId.EnemyTurn, new BattleStateEnemyTurn() },
            { EBattleStateId.GameOver, new BattleStateGameOver() }
        };
        
        private readonly Stack<EBattleStateId> _stateStack = new();

        public BattleStateMachine(BattleInstanceClient battleInstance, GameFieldSetup gameFieldSetup,
            UserDataService userDataService, LobbyService lobbyService, NetworkService networkService)
        {
            BattleInstance = battleInstance;
            GameFieldSetup = gameFieldSetup;
            UserDataService = userDataService;
            LobbyService = lobbyService;
            NetworkService = networkService;
        }

        public BattleState CurrentState { get; private set; } = new NullBattleState();
        public bool IsInTransition { get; private set; }
        public BattleInstanceClient BattleInstance { get; }
        public UserDataService UserDataService { get; }
        public LobbyService LobbyService { get; }
        public NetworkService NetworkService { get; }
        public GameFieldSetup GameFieldSetup { get; }

        public event Action<BattleState, BattleState> OnStateChanged;
        public UnityEvent<BattleState, BattleState> OnStateEntered { get; } = new();

        public void Init()
        {
            foreach (var kvp in _states)
            {
                kvp.Value.Setup(this);
            }
        }

        public void Terminate()
        {
            CurrentState.Exit();
            _states.Clear();
            _stateStack.Clear();
        }

        public void ChangeState(EBattleStateId battleStateId)
        {
            Debug.Log($"Changing state to {battleStateId}");
            if (CurrentState.Id == battleStateId) return;

            if (IsInTransition) return;

            IsInTransition = true;
            var oldState = CurrentState;
            CurrentState.Exit();
            CurrentState = _states[battleStateId];
            CurrentState.Enter();
            var newState = CurrentState;
            IsInTransition = false;
            OnStateChanged?.Invoke(oldState, newState);
            OnStateEntered.Invoke(oldState, newState);
        }

        public void PushState(EBattleStateId battleStateId)
        {
            _stateStack.Push(CurrentState.Id);
            ChangeState(battleStateId);
        }

        public void PopState()
        {
            if (_stateStack.Count == 0) return;
            ChangeState(_stateStack.Pop());
        }
    }
}