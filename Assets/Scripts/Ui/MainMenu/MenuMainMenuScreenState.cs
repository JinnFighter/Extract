using Common;
using FishNet;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;
using VContainer;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

namespace Ui.MainMenu
{
    public class MenuMainMenuScreenState : ScreenState
    {
        [field: SerializeField] public Button ButtonHost { get; private set; }
        [field: SerializeField] public Button ButtonJoin { get; private set; }
        [field: SerializeField] public TMP_InputField TextFieldNickname { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextConnectionInfo { get; private set; }
        [Inject] private LoadingService _loadingService;
        [Inject] private LobbyService _lobbyService;

        [Inject] private NetworkService _networkService;

        protected override void EnterStatInner()
        {
            ButtonHost.onClick.AddListener(HandleButtonHostClicked);
            ButtonJoin.onClick.AddListener(HandleButtonJoinClicked);
            TextFieldNickname.text = GetNickname();
            TextFieldNickname.onEndEdit.AddListener(HandleTextFieldNicknameEndEdit);
            _lobbyService.Players.OnChange += HandlePlayersChanged;
        }

        protected override void ExitStatInner()
        {
            ButtonHost.onClick.RemoveListener(HandleButtonHostClicked);
            ButtonJoin.onClick.RemoveListener(HandleButtonJoinClicked);
            TextFieldNickname.onEndEdit.RemoveListener(HandleTextFieldNicknameEndEdit);
            _lobbyService.Players.OnChange -= HandlePlayersChanged;
        }

        private void HandlePlayersChanged(SyncListOperation op, int index, PlayerData olditem, PlayerData newitem,
            bool asserver)
        {
            if (op != SyncListOperation.Add || index < 0) return;
            if (_lobbyService.Players.Count > 1)
                if (InstanceFinder.IsHostStarted && _lobbyService.Players.Count > 1)
                    _loadingService.LoadScene("TestBattle", true);
        }

        private void HandleTextFieldNicknameEndEdit(string arg0)
        {
            PlayerPrefs.SetString("Nickname", arg0);
            PlayerPrefs.Save();
        }

        private async void HandleButtonHostClicked()
        {
            ButtonHost.gameObject.SetActive(false);
            ButtonJoin.gameObject.SetActive(false);
            TextConnectionInfo.gameObject.SetActive(true);
            await _networkService.Host();
            await _networkService.Connect();
        }

        private async void HandleButtonJoinClicked()
        {
            ButtonHost.gameObject.SetActive(false);
            ButtonJoin.gameObject.SetActive(false);
            TextConnectionInfo.gameObject.SetActive(true);
            await _networkService.Connect();
        }

        private string GetNickname()
        {
            var nickname = "Unknown";
            if (PlayerPrefs.HasKey("Nickname"))
            {
                nickname = PlayerPrefs.GetString("Nickname");
                PlayerPrefs.Save();
            }
            else
            {
                nickname = $"Player_{Random.Range(1000, 9999)}";
                PlayerPrefs.SetString("Nickname", nickname);
            }

            return nickname;
        }
    }
}