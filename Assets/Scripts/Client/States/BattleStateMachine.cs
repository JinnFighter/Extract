using System;
using System.Collections.Generic;
using Common;
using Logic;
using Logic.States;
using UnityEngine;

namespace Client.States
{
    public class BattleStateMachine
    {
        private readonly Dictionary<EBattleStateId, BattleState> _states = new()
        {
            { EBattleStateId.Init, new BattleStateInit() },
            { EBattleStateId.PlayerTurn, new BattleStatePlayerTurn() },
            { EBattleStateId.EnemyTurn, new BattleStateEnemyTurn() },
            { EBattleStateId.GameOver, new BattleStateGameOver() }
        };

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
        }

        public void ChangeState(EBattleStateId eBattleStateId)
        {
            Debug.Log($"Changing state to {eBattleStateId}");
            if (CurrentState.Id == eBattleStateId) return;

            if (IsInTransition) return;

            IsInTransition = true;
            var oldState = CurrentState;
            CurrentState.Exit();
            CurrentState = _states[eBattleStateId];
            CurrentState.Enter();
            var newState = CurrentState;
            IsInTransition = false;
            OnStateChanged?.Invoke(oldState, newState);
        }
    }
}