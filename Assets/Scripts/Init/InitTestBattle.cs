using Client;
using Client.Actions;
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
        [SerializeField] private BattleInstanceClient _battleInstanceClient;
        [SerializeField] private UnitSpawner _unitSpawner;
        [Inject] private LobbyService _lobbyService;
        [Inject] private NetworkService _networkService;
        [Inject] private UserDataService _userDataService;
        [Inject] private IUiService _uiService;
        [Inject] private IUnitEntitySelectorSystem _unitEntitySelectorSystem;
        [Inject] private IActionRequestBuilderSystem _actionRequestBuilderSystem;
        [Inject] private IActionRequestSender _actionRequestSender;
        [SerializeField] private ReplayService _replayService;
        private BattleScreenModel _battleScreenModel;

        private void Start()
        {
            _unitSpawner.Init();
            _battleInstance.Init();
            _replayService.Init();
            _battleInstanceClient.Init();
            _battleScreenModel = new BattleScreenModel(new BattleScreenStateModelAlly
                {
                    LogicModelClient = _battleInstanceClient.ModelClient,
                    ActionRequestSender = _actionRequestSender,
                    UserDataService = _userDataService,
                    ActionRequestBuilderSystem = _actionRequestBuilderSystem,
                },
                new BattleScreenStateModelEnemy(), _battleInstanceClient, _userDataService, new ModelWidgetSelectedUnit
                {
                    SelectorSystem = _unitEntitySelectorSystem,
                },new WidgetSelectedUnitOptionsModel()
                {
                    SelectorSystem = _unitEntitySelectorSystem,
                });
            _uiService.Open<UiBattleScreen>(_battleScreenModel, typeof(BattleScreenView));
        }

        private void OnDestroy()
        {
            _uiService.Close(_battleScreenModel);
            _battleScreenModel = null;
            _replayService?.Terminate();
            _battleInstanceClient?.Terminate();
            _battleInstance?.Terminate();
            _unitSpawner?.Terminate();
        }
    }
}