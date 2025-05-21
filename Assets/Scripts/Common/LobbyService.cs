using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace Common
{
    public class LobbyService : NetworkBehaviour
    {
        public SyncList<PlayerData> Players { get; } = new();
    }
}