using TMPro;
using UiService.Code.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.MainMenu
{
    public class MainMenuScreenStateView : UiView
    {
        [field: SerializeField] public Button ButtonHost { get; private set; }
        [field: SerializeField] public Button ButtonJoin { get; private set; }
        [field: SerializeField] public Button ButtonCoterie { get; private set; }
        [field: SerializeField] public TMP_InputField TextFieldNickname { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextConnectionInfo { get; private set; }
        [field: SerializeField] public TMP_InputField TextFieldIp { get; private set; }
    }
}