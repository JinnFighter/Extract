using System.Collections.Generic;
using FishNet.Object.Synchronizing;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Ui.MainMenu
{
    public class LobbyMainMenuScreenState : ScreenState
    {
        private List<PlayerLobbyView> _playerLobbyViews = new();
        [field: SerializeField] public Button ButtonStartGame { get; private set; }
        [field: SerializeField] public Transform PlayersContainer { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextWaitForHost { get; private set; }
        
        [SerializeField] private PlayerLobbyView _playerLobbyViewPrefab;

        [Inject] private NetworkService _networkService;
        [Inject] private LobbyService _lobbyService;

        protected override void EnterStatInner()
        {
            if (_networkService.IsHosting())
            {
                ButtonStartGame.gameObject.SetActive(true);
                TextWaitForHost.gameObject.SetActive(false);
            }
            else if (_networkService.IsClientConnected())
            {
                ButtonStartGame.gameObject.SetActive(false);
                TextWaitForHost.gameObject.SetActive(true);
            }
            
            _lobbyService.Players.OnChange += HandlePlayersChanged;
        }

        protected override void ExitStatInner()
        {
            _lobbyService.Players.OnChange -= HandlePlayersChanged;
        }

        private void HandlePlayersChanged(SyncListOperation op, int index, PlayerData olditem, PlayerData newitem, bool asserver)
        {
            switch (op)
            {
                case SyncListOperation.Add:
                    if(index < 0)
                        return;
                    AddPlayer(newitem);
                    break;
                case SyncListOperation.RemoveAt:
                    RemovePlayer(olditem, index);
                    break;
            }
        }

        private void AddPlayer(PlayerData player)
        {
            var lobbyView = Instantiate(_playerLobbyViewPrefab, PlayersContainer, false);
            lobbyView.SetPlayerData(player);
            _playerLobbyViews.Add(lobbyView);
        }

        private void RemovePlayer(PlayerData player, int index)
        {
            var lobbyView = _playerLobbyViews[index];
            lobbyView.ResetPlayerData();
            _playerLobbyViews.RemoveAt(index);
            Destroy(lobbyView);
        }
    }
}