using TMPro;
using UiService.Code.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Battle
{
    public class BattleScreenStateViewGameOver : UiView
    {
        [field: SerializeField] public TextMeshProUGUI TextWinnerId { get; private set; }
        [field: SerializeField] public Button ButtonQuitGame { get; private set; }
    }
}
