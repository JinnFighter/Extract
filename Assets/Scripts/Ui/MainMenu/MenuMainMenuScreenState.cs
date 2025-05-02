using Network;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Ui.MainMenu
{
    public class MenuMainMenuScreenState : ScreenState
    {
        [SerializeField] private LobbyMainMenuScreenState _lobbyMainMenuScreenState;
        
        [Inject]
        private NetworkService _networkService;
        [field: SerializeField] public Button ButtonHost { get; private set; }
        [field: SerializeField] public Button ButtonJoin { get; private set; }

        protected override void EnterStatInner()
        {
            ButtonHost.onClick.AddListener(HandleButtonHostClicked);
            ButtonJoin.onClick.AddListener(HandleButtonJoinClicked);
        }

        protected override void ExitStatInner()
        {
            ButtonHost.onClick.RemoveListener(HandleButtonHostClicked);
            ButtonJoin.onClick.RemoveListener(HandleButtonJoinClicked);
        }

        private async void HandleButtonHostClicked()
        {
            await _networkService.Host();
            await _networkService.Connect();
            Owner.SwitchState(_lobbyMainMenuScreenState);
        }

        private async void HandleButtonJoinClicked()
        {
            await _networkService.Connect();
            Owner.SwitchState(_lobbyMainMenuScreenState);
        }
    }
}