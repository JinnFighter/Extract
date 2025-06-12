using Common;
using Logic;
using MVVM;

namespace Ui.Battle
{
    public class BattleScreenStateModelAlly : IModel
    {
        public BattleInstanceModel BattleInstanceModel { get; set; }
        public UserDataService UserDataService { get; set; }
        public IActionRequestSender ActionRequestSender { get; set; }
    }
}