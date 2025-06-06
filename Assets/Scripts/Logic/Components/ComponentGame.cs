using System.Collections.Generic;

namespace Logic.Components
{
    public struct ComponentGame
    {
        public int CurrentPlayerId => PlayerIds[CurrentPlayerIndex];
        public int CurrentPlayerIndex;
        public List<int> PlayerIds;
        public int WinnerId;
        public bool IsGameOver;
    }
}
