using UiService.Code.Widgets;
using UnityEngine;

namespace Ui.Battle
{
    public class ViewWidgetSelectedUnit : UiView
    {
        [field: SerializeField] public GameObject UnitInfoPanel { get; private set; }
    }
}