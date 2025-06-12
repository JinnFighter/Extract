using Client;
using Client.States;
using Common;
using Logic.States;
using MVVM;

namespace Ui.Battle
{
    public class BattleScreenModel : IModel
    {
        public BattleStateMachine BattleStateMachine { get; private set; }
        public UserDataService UserDataService { get; private set; }
        public BattleScreenModel(BattleScreenStateModelAlly modelAlly, BattleScreenStateModelEnemy modelEnemy, BattleInstanceClient battleInstance, UserDataService userDataService)
        {
            ModelAlly = modelAlly;
            ModelEnemy = modelEnemy;
            BattleStateMachine = battleInstance.StateMachine;
            UserDataService = userDataService;
            GameOverModel = new BattleScreenStateGameOverModel(battleInstance.ModelClient);
        }

        public BattleScreenStateModelAlly ModelAlly { get; private set; }
        public BattleScreenStateModelEnemy ModelEnemy { get; private set; }
        public BattleScreenStateGameOverModel GameOverModel { get; private set; }
    }
}