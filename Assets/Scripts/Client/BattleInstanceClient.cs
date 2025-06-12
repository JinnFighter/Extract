using Client.GameStateEvents;
using Client.States;
using Common;
using FishNet.Transporting;
using Logic;
using Logic.ActionRequests;
using Logic.GameStateEvents;
using Logic.States;
using UnityEngine;
using VContainer;

namespace Client
{
    public class BattleInstanceClient : MonoBehaviour, IActionRequestSender
    {
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        [Inject] private NetworkService _networkService;
        public readonly ActionEventListener ActionEventListener = new();
        [Inject] private LobbyService _lobbyService;
        [Inject] private UserDataService _userDataService;
        public LogicModelClient ModelClient { get; } = new();
        public BattleStateMachine StateMachine { get; private set; }

        public void Init()
        {
            StateMachine =
                new BattleStateMachine(this, _gameFieldSetup, _userDataService, _lobbyService, _networkService);
            StateMachine.Init();
            ActionEventListener.Init(this, _networkService);
            _networkService.SubscribeClientBroadcast<BroadcastActionEvent>(HandleBroadcastGameStateEvent);
        }
        
        public void Terminate()
        {
            StateMachine?.Terminate();
            _networkService.UnsubscribeClientBroadcast<BroadcastActionEvent>(HandleBroadcastGameStateEvent);
            ActionEventListener.Terminate();
        }

        private void HandleBroadcastGameStateEvent(BroadcastActionEvent arg1, Channel arg2)
        {ActionEventListener.AddEventToQueue(arg1.ActionEvent);
        }
        
        public void SendActionRequest<T>(T actionRequest) where T : ActionRequest
        {
            _networkService.SendClientBroadcast(new ActionRequestBroadcast
            {
                ActionRequest = actionRequest
            });
        }
    }
}
