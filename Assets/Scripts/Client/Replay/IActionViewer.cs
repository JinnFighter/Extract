using System.Collections.Generic;
using Logic;
using Logic.GameStateEvents;

namespace Client.Replay
{
    public interface IActionViewer
    {
        IEnumerator<bool> Play(BattleInstance battleInstance, GameStateEvent gameStateEvent);
    }
}