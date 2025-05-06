using System;
using Network;
using TMPro;
using UnityEngine;
using VContainer;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

namespace Ui.MainMenu
{
    public class MenuMainMenuScreenState : ScreenState
    {
        [SerializeField] private LobbyMainMenuScreenState _lobbyMainMenuScreenState;
        [field: SerializeField] public Button ButtonHost { get; private set; }
        [field: SerializeField] public Button ButtonJoin { get; private set; }
        [field: SerializeField] public TMP_InputField TextFieldNickname { get; private set; }

        [Inject] private NetworkService _networkService;

        protected override void EnterStatInner()
        {
            ButtonHost.onClick.AddListener(HandleButtonHostClicked);
            ButtonJoin.onClick.AddListener(HandleButtonJoinClicked);
            TextFieldNickname.text = GetNickname();
            TextFieldNickname.onEndEdit.AddListener(HandleTextFieldNicknameEndEdit);
        }

        private void HandleTextFieldNicknameEndEdit(string arg0)
        {
            PlayerPrefs.SetString("Nickname", arg0);
            PlayerPrefs.Save();
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