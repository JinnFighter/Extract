using System.Collections.Generic;
using Logic;
using Logic.ActionEvents;

namespace Client.Replay
{
    public interface IActionViewer
    {
        IEnumerator<bool> Play(BattleInstanceClient battleInstance, ActionEvent actionEvent);
    }
}