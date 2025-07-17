using System.Collections.Generic;
using UiService.Code.Widgets;
using UnityEngine;

namespace Ui.Battle
{
    public class WidgetViewSelectedUnitOptions : UiView
    {
        [field: SerializeField] public GameObject Panel { get; private set; }
        [field: SerializeField] public List<WidgetViewActionOption> Options { get; private set; }
    }
}