using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Ui.MainMenu
{
    public class LobbyMainMenuScreenState : ScreenState
    {
        [field: SerializeField] public Button ButtonStartGame { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextWaitForHost { get; private set; }

        [Inject] private NetworkService _networkService;

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
        }
    }
}