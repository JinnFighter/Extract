using UiService.Code.Widgets;
using UnityEngine;

namespace Ui.MainMenu
{
    public class MainMenuScreenView : UiView
    {
        [field: SerializeField] public MainMenuScreenStateView MainMenuStateView { get; private set; }
    }
}