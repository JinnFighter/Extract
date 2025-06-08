using Logic;
using MVVM;

namespace Ui.Battle
{
    public class BattleScreenStateGameOverModel : IModel
    {
        public BattleScreenStateGameOverModel(BattleInstanceModel battleInstanceModel)
        {
            BattleInstanceModel = battleInstanceModel;
        }

        public BattleInstanceModel BattleInstanceModel { get; set; }
    }
}