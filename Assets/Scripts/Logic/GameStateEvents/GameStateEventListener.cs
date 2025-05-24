using System;
using System.Collections.Generic;
using Common;
using FishNet.Transporting;
using UnityEngine;

namespace Logic.GameStateEvents
{
    public class GameStateEventListener
    {
        private NetworkService _networkService;
        public bool IsListening { get; private set; }

        public Queue<IGameStateEvent> GameEventQueue { get; } = new();
        
        public event Action OnGameEventsReceived;

        public void Init(NetworkService networkService)
        {
            _networkService = networkService;
            SubscribeToGameStateEvents();
        }

        public void Terminate()
        {
            UnsubscribeFromGameStateEvents();
            _networkService = null;
        }

        private void SubscribeToGameStateEvents()
        {
            _networkService.SubscribeClientBroadcast<GameStateEventGameStarted>(HandleGameStateEventGameStartedReceived);
            _networkService.SubscribeClientBroadcast<GameStateEventActivePlayerChanged>(
                HandleGameStateEventPlayerChangedReceived);
            _networkService.SubscribeClientBroadcast<GameStateEventGameEnded>(HandleGameStateEventGameEndedReceived);
            IsListening = true;
        }

        private void UnsubscribeFromGameStateEvents()
        {
            _networkService.UnsubscribeClientBroadcast<GameStateEventGameStarted>(HandleGameStateEventGameStartedReceived);
            _networkService.UnsubscribeClientBroadcast<GameStateEventActivePlayerChanged>(
                HandleGameStateEventPlayerChangedReceived);
            _networkService.UnsubscribeClientBroadcast<GameStateEventGameEnded>(HandleGameStateEventGameEndedReceived);
            IsListening = false;
        }
        
        private void HandleGameStateEventGameStartedReceived(GameStateEventGameStarted arg1, Channel arg2)
        {
            AddEventToQueue(arg1);
        }

        private void HandleGameStateEventPlayerChangedReceived(GameStateEventActivePlayerChanged arg1, Channel arg2)
        {
            AddEventToQueue(arg1);
        }

        private void HandleGameStateEventGameEndedReceived(GameStateEventGameEnded arg1, Channel arg2)
        {
            AddEventToQueue(arg1);
        }

        private void AddEventToQueue(IGameStateEvent gameEvent)
        {
            Debug.Log($"Received Event : {gameEvent} ");
            GameEventQueue.Enqueue(gameEvent);
            OnGameEventsReceived?.Invoke();
        }
    }
}