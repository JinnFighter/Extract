using System.Collections.Generic;
using Logic;
using Logic.GameStateEvents;

namespace Client.Replay
{
    public interface IActionViewer
    {
        IEnumerator<bool> Play(BattleInstanceClient battleInstance, ActionEvent actionEvent);
    }
}