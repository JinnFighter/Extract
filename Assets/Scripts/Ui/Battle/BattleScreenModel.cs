using Common;
using Logic;
using Logic.States;
using MVVM;

namespace Ui.Battle
{
    public class BattleScreenModel : IModel
    {
        public BattleStateMachine BattleStateMachine { get; private set; }
        public UserDataService UserDataService { get; private set; }
        public IActionRequestSender ActionRequestSender { get; private set; }
        public BattleScreenModel(BattleScreenStateModelAlly modelAlly, BattleScreenStateModelEnemy modelEnemy, BattleInstance battleInstance, UserDataService userDataService)
        {
            ModelAlly = modelAlly;
            ModelEnemy = modelEnemy;
            BattleStateMachine = battleInstance.StateMachine;
            ActionRequestSender = battleInstance;
            UserDataService = userDataService;
            GameOverModel = new BattleScreenStateGameOverModel(battleInstance.Model);
        }

        public BattleScreenStateModelAlly ModelAlly { get; private set; }
        public BattleScreenStateModelEnemy ModelEnemy { get; private set; }
        public BattleScreenStateGameOverModel GameOverModel { get; private set; }
    }
}