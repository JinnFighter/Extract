using System.Collections.Generic;

namespace Logic.Entities
{
    public class GameEntityServer : BaseEntityServer
    {
        public List<int> PlayerIds { get; } = new();
        public int CurrentPlayerIndex { get; set; }
        public int CurrentPlayerId => PlayerIds[CurrentPlayerIndex];
        public int WinnerId;
        public bool IsGameOver;
    }
}