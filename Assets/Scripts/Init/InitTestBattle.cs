using System.Linq;
using Client;
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
        [SerializeField] private UnitSpawner _unitSpawner;
        [Inject] private LobbyService _lobbyService;
        [Inject] private NetworkService _networkService;
        [Inject] private UserDataService _userDataService;

        private async void Start()
        {
            _userDataService.LocalPlayer.SetReady(true);
            await UniTask.WaitUntil(() => _lobbyService.Players.All(player => player.IsReady));
            _unitSpawner.Init();
            _battleInstance.Init();
        }

        private void OnDestroy()
        {
            _battleInstance?.Terminate();
            _unitSpawner?.Terminate();
        }
    }
}