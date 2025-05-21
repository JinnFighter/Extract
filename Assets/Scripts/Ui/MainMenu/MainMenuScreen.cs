using UnityEngine;

namespace Ui.MainMenu
{
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] private MenuMainMenuScreenState _menuState;

        protected override void InitInner()
        {
            _menuState.SetOwner(this);
            _menuState.gameObject.SetActive(false);
            SwitchState(_menuState);
        }
    }
}