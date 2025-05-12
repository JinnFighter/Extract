using FishNet.Component.Spawning;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Network
{
    public class LobbyService : NetworkBehaviour
    {
        [SerializeField] private PlayerSpawner _playerSpawner;

        public SyncList<PlayerData> Players { get; } = new();
        public PlayerData LocalPlayer { get; private set; }

        private void Awake()
        {
            _playerSpawner.OnSpawned += PlayerSpawnerOnOnSpawned;
            Players.OnChange += HandlePlayersChanged;
        }

        private void HandlePlayersChanged(SyncListOperation op, int index, PlayerData olditem, PlayerData newitem, bool asserver)
        {
            switch (op)
            {
                case SyncListOperation.Add:
                    if(OwnerId == newitem.OwnerId)
                    {
                        LocalPlayer = newitem;
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnDestroy()
        {
            _playerSpawner.OnSpawned -= PlayerSpawnerOnOnSpawned;
            Players.OnChange -= HandlePlayersChanged;
        }

        private void PlayerSpawnerOnOnSpawned(NetworkObject obj)
        {
            Players.Add(obj.GetComponent<PlayerData>());
        }
    }
}