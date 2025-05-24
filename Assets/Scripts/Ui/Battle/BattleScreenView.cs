using UiService.Code.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Battle
{
    public class BattleScreenView : UiView
    {
        [field: SerializeField] public BattleScreenStateViewAlly ScreenStateViewAlly { get; set; }
        [field: SerializeField] public BattleScreenStateViewEnemy ScreenStateViewEnemy { get; set; }
        [field: SerializeField] public Button ButtonEndTurn { get; private set; }
    }
}