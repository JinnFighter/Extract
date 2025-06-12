using UiService.Code.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Battle
{
    public class BattleScreenView : UiView
    {
        [field: SerializeField] public BattleScreenStateViewAlly ScreenStateViewAlly { get; private set; }
        [field: SerializeField] public BattleScreenStateViewEnemy ScreenStateViewEnemy { get; private set; }
        [field: SerializeField] public BattleScreenStateViewGameOver ScreenStateViewGameOver { get; private set; }
        [field: SerializeField] public ViewWidgetSelectedUnit SelectedUnit { get; private set; }
    }
}