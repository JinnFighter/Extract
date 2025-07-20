using TMPro;
using UiService.Code.Widgets;
using UnityEngine;

namespace Ui.MainMenu
{
    public class WidgetCoterieUnitView : UiView
    {
        [field: SerializeField] public TMP_Dropdown Dropdown { get; private set; }
    }
}
