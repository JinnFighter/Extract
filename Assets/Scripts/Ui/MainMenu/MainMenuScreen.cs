using UnityEngine;

namespace Ui.MainMenu
{
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] private MenuMainMenuScreenState _menuState;

        protected override void InitInner()
        {
            SwitchState(_menuState);
        }
    }
}