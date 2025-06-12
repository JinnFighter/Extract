using Client;
using Common;
using Logic;
using MVVM;

namespace Ui.Battle
{
    public class BattleScreenStateModelAlly : IModel
    {
        public LogicModelClient LogicModelClient { get; set; }
        public UserDataService UserDataService { get; set; }
        public IActionRequestSender ActionRequestSender { get; set; }
    }
}