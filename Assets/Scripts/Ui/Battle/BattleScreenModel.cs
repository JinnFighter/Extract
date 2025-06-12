using Client;
using Client.States;
using Common;
using MVVM;

namespace Ui.Battle
{
    public class BattleScreenModel : IModel
    {
        public BattleStateMachine BattleStateMachine { get; private set; }
        public UserDataService UserDataService { get; private set; }
        public BattleScreenModel(BattleScreenStateModelAlly modelAlly, BattleScreenStateModelEnemy modelEnemy, BattleInstanceClient battleInstance, UserDataService userDataService, ModelWidgetSelectedUnit widgetSelectedUnit)
        {
            ModelAlly = modelAlly;
            ModelEnemy = modelEnemy;
            BattleStateMachine = battleInstance.StateMachine;
            UserDataService = userDataService;
            GameOverModel = new BattleScreenStateGameOverModel(battleInstance.ModelClient);
            ModelWidgetSelectedUnit = widgetSelectedUnit;
        }

        public BattleScreenStateModelAlly ModelAlly { get; private set; }
        public BattleScreenStateModelEnemy ModelEnemy { get; private set; }
        public BattleScreenStateGameOverModel GameOverModel { get; private set; }
        public ModelWidgetSelectedUnit ModelWidgetSelectedUnit { get; private set; }
    }
}