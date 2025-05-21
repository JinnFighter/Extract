using System.Linq;
using Common;
using Cysharp.Threading.Tasks;
using Logic;
using UnityEngine;
using VContainer;

namespace Init
{
    public class InitTestBattle : MonoBehaviour
    {
        [SerializeField] private GameFieldSetup _gameFieldSetup;
        [SerializeField] private BattleInstance _battleInstance;
        [Inject] private LobbyService _lobbyService;
        [Inject] private NetworkService _networkService;
        [Inject] private UserDataService _userDataService;

        private async void Start()
        {
            _userDataService.LocalPlayer.SetReady(true);
            if (_networkService.IsHosting())
                await UniTask.WaitUntil(() => _lobbyService.Players.All(player => player.IsReady));
            _battleInstance.Init();
        }

        private void OnDestroy()
        {
            _battleInstance?.Terminate();
        }
    }
}