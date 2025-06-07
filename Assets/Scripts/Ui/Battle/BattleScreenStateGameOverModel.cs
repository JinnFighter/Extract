using Logic;
using MVVM;

namespace Ui.Battle
{
    public class BattleScreenStateGameOverModel : IModel
    {
        public BattleScreenStateGameOverModel(BattleInstance battleInstance)
        {
            BattleInstance = battleInstance;
        }

        public BattleInstance BattleInstance { get; set; }
    }
}