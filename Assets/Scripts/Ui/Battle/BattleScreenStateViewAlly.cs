using UiService.Code.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Battle
{
    public class BattleScreenStateViewAlly : UiView
    {
        [field: SerializeField] public Button ButtonEndTurn { get; private set; }
    }
}