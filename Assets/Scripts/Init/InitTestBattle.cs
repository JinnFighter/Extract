using Client;
using Client.Replay;
using Common;
using Logic;
using Ui.Battle;
using UiService;
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
        [Inject] private IUiService _uiService;
        [SerializeField] private ReplayService _replayService;
        private BattleScreenModel _battleScreenModel;

        private void Start()
        {
            _unitSpawner.Init();
            _battleInstance.Init();
            _replayService.Init();
            _battleScreenModel = new BattleScreenModel(new BattleScreenStateModelAlly
                {
                    BattleInstanceModel = _battleInstance.Model,
                    ActionRequestSender = _battleInstance,
                    UserDataService = _userDataService
                },
                new BattleScreenStateModelEnemy(), _battleInstance, _userDataService);
            _uiService.Open<UiBattleScreen>(_battleScreenModel, typeof(BattleScreenView));
        }

        private void OnDestroy()
        {
            _uiService.Close(_battleScreenModel);
            _battleScreenModel = null;
            _replayService?.Terminate();
            _battleInstance?.Terminate();
            _unitSpawner?.Terminate();
        }
    }
}