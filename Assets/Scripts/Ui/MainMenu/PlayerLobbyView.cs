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
    }
}
