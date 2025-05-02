using Ui.MainMenu;
using UnityEngine;

namespace Init
{
    public class InitMainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuScreen _mainMenuScreen;

        private void Start()
        {
            _mainMenuScreen.Init();
        }

        private void OnDestroy()
        {
            _mainMenuScreen.Terminate();
        }
    }
}