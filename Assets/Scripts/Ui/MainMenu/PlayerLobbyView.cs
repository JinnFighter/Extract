using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.MainMenu
{
    public class PlayerLobbyView : MonoBehaviour
    {
        [field: SerializeField] public Button ButtonReady { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextNickname { get; private set; }
        [field: SerializeField] public Toggle ToggleReady { get; private set; }

        private IPlayerData _playerData;

        public void SetPlayerData(IPlayerData playerData)
        {
            if (_playerData != null) return;
            _playerData = playerData;
            _playerData.OnNicknameUpdated += HandleNicknameUpdated;
            HandleNicknameUpdated(_playerData.Nickname);
            _playerData.OnReadyUpdated += HandleReadyUpdated;
            HandleReadyUpdated(_playerData.IsReady);

            if (_playerData.IsLocalPlayer)
            {
                ButtonReady.gameObject.SetActive(true);
                ButtonReady.onClick.AddListener(HandleButtonReadyClicked);
            }
            else
            {
                ButtonReady.gameObject.SetActive(false);
            }
        }

        public void ResetPlayerData()
        {
            if (_playerData == null) return;

            _playerData.OnNicknameUpdated -= HandleNicknameUpdated;
            _playerData.OnReadyUpdated -= HandleReadyUpdated;
            if (_playerData.IsLocalPlayer)
            {
                ButtonReady.onClick.RemoveListener(HandleButtonReadyClicked);
            }
            _playerData = null;
        }

        private void HandleButtonReadyClicked()
        {
            _playerData.SetReady(!_playerData.IsReady);
        }

        private void HandleReadyUpdated(bool obj)
        {
            ToggleReady.isOn = obj;
        }

        private void HandleNicknameUpdated(string obj)
        {
            TextNickname.text = obj;
        }
    }
}