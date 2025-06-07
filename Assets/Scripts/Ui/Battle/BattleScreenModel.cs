using Common;
using Logic;
using MVVM;

namespace Ui.Battle
{
    public class BattleScreenModel : IModel
    {
        public BattleInstance BattleInstance { get; private set; }
        public UserDataService UserDataService { get; private set; }
        public BattleScreenModel(BattleScreenStateModelAlly modelAlly, BattleScreenStateModelEnemy modelEnemy, BattleInstance battleInstance, UserDataService userDataService)
        {
            ModelAlly = modelAlly;
            ModelEnemy = modelEnemy;
            BattleInstance = battleInstance;
            UserDataService = userDataService;
            GameOverModel = new BattleScreenStateGameOverModel(battleInstance);
        }

        public BattleScreenStateModelAlly ModelAlly { get; private set; }
        public BattleScreenStateModelEnemy ModelEnemy { get; private set; }
        public BattleScreenStateGameOverModel GameOverModel { get; private set; }
    }
}