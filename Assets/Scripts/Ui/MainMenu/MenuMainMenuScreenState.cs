using UnityEngine;
using UnityEngine.UI;

namespace Ui.MainMenu
{
    public class MenuMainMenuScreenState : ScreenState
    {
        [field: SerializeField] public Button ButtonHost { get; private set; }
        [field: SerializeField] public Button ButtonJoin { get; private set; }

        protected override void EnterStatInner()
        {
            ButtonHost.onClick.AddListener(HandleButtonHostClicked);
            ButtonJoin.onClick.AddListener(HandleButtonJoinClicked);
        }

        protected override void ExitStatInner()
        {
            ButtonHost.onClick.RemoveListener(HandleButtonHostClicked);
            ButtonJoin.onClick.RemoveListener(HandleButtonJoinClicked);
        }

        private void HandleButtonHostClicked()
        {
            Debug.Log("HandleButtonHostClicked");
        }

        private void HandleButtonJoinClicked()
        {
            Debug.Log("HandleButtonJoinClicked");
        }
    }
}