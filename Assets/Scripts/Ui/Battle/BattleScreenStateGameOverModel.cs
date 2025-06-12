using Client;
using MVVM;

namespace Ui.Battle
{
    public class BattleScreenStateGameOverModel : IModel
    {
        public BattleScreenStateGameOverModel(LogicModelClient logicModelClient)
        {
            LogicModelClient = logicModelClient;
        }

        public LogicModelClient LogicModelClient { get; set; }
    }
}