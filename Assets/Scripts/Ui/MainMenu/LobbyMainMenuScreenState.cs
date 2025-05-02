using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.MainMenu
{
    public class LobbyMainMenuScreenState : ScreenState
    {
        [field: SerializeField] public Button ButtonStartGame { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextWaitForHost { get; private set; }
    }
}
