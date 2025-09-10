using System.Collections.Generic;
using Logic.ActionEvents;

namespace Client.Replay
{
    public interface IActionViewer
    {
        void Prepare(BattleInstanceClient battleInstance);
        IEnumerator<bool> Play(BattleInstanceClient battleInstance, ActionEvent actionEvent);
        void Finish(BattleInstanceClient battleInstance);
    }
}