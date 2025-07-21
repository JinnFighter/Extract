using Common;
using FishNet;
using FishNet.Object.Synchronizing;
using UiService.Code.Widgets;
using VContainer;
using Random = UnityEngine.Random;

namespace Ui.MainMenu
{
    public class MenuMainMenuScreenState : BaseUiScreenState<MainMenuScreenStateModel, MainMenuScreenStateView>
    {
        protected override void InitInner()
        {
            View.ButtonHost.onClick.AddListener(HandleButtonHostClicked);
            View.ButtonJoin.onClick.AddListener(HandleButtonJoinClicked);
            View.ButtonCoterie.onClick.AddListener(HandleButtonCoterieClicked);
            View.TextFieldNickname.text = Model.UserDataService.LocalPlayer.Nickname;
            View.TextFieldNickname.onEndEdit.AddListener(HandleTextFieldNicknameEndEdit);
            Model.LobbyService.Players.OnChange += HandlePlayersChanged;
        }

        

        protected override void TerminateInner()
        {
            View.ButtonHost.onClick.RemoveListener(HandleButtonHostClicked);
            View.ButtonJoin.onClick.RemoveListener(HandleButtonJoinClicked);
            View.TextFieldNickname.onEndEdit.RemoveListener(HandleTextFieldNicknameEndEdit);
            Model.LobbyService.Players.OnChange -= HandlePlayersChanged;
        }

        private void HandlePlayersChanged(SyncListOperation op, int index, PlayerData olditem, PlayerData newitem,
            bool asserver)
        {
            if (op != SyncListOperation.Add || index < 0) return;
            if (Model.LobbyService.Players.Count > 1)
                if (InstanceFinder.IsHostStarted && Model.LobbyService.Players.Count > 1)
                    Model.LoadingService.LoadScene("TestBattle", true);
        }

        private void HandleTextFieldNicknameEndEdit(string arg0)
        {
            Model.UserDataService.LocalPlayer.SetNickname(arg0);
        }

        private async void HandleButtonHostClicked()
        {
            View.ButtonHost.gameObject.SetActive(false);
            View.ButtonJoin.gameObject.SetActive(false);
            View.ButtonCoterie.gameObject.SetActive(false);
            View.TextConnectionInfo.gameObject.SetActive(true);
            await Model.NetworkService.Host();
            await Model.NetworkService.Connect();
        }

        private async void HandleButtonJoinClicked()
        {
            View.ButtonHost.gameObject.SetActive(false);
            View.ButtonJoin.gameObject.SetActive(false);
            View.ButtonCoterie.gameObject.SetActive(false);
            View.TextConnectionInfo.gameObject.SetActive(true);
            await Model.NetworkService.Connect();
        }
        
        private void HandleButtonCoterieClicked()
        {
            StateRouter.PushState<CoterieMainMenuScreenState>();
        }
    }
}