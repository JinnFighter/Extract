using UnityEngine;

namespace Ui.MainMenu
{
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] private MenuMainMenuScreenState _menuState;
        [SerializeField] private LobbyMainMenuScreenState _lobbyState;

        protected override void InitInner()
        {
            _menuState.SetOwner(this);
            _menuState.gameObject.SetActive(false);
            _lobbyState.SetOwner(this);
            _lobbyState.gameObject.SetActive(false);
            SwitchState(_menuState);
        }
    }
}