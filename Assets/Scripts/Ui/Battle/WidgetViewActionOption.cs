using TMPro;
using UiService.Code.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Battle
{
    public class WidgetViewActionOption : UiView
    {
        [field: SerializeField] public Button ButtonCallAction { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextActionName { get; private set; }
    }
}