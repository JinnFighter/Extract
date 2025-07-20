using System.Collections.Generic;
using UiService.Code.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.MainMenu
{
    public class CoterieMainMenuStateView : UiView
    {
        [field: SerializeField] public List<WidgetCoterieUnitView> UnitViews { get; private set; }
        [field: SerializeField] public Button ButtonBack { get; private set; }
    }
}