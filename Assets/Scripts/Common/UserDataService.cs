using FishNet.Object;
using UnityEngine;
using VContainer;

namespace Common
{
    public class UserDataService : MonoBehaviour
    {
        private readonly PlayerDataLocal _playerDataLocal = new();
        [Inject] private NetworkService _networkService;
        public IPlayerData LocalPlayer => _playerDataLocal;

        private void Awake()
        {
            _networkService.OnClientConnected += HandleClientConnected;
        }

        private void OnDestroy()
        {
            _networkService.OnClientConnected -= HandleClientConnected;
            _playerDataLocal.ResetNetPlayerData();
        }

        private void HandleClientConnected(NetworkObject obj)
        {
            var playerData = obj.GetComponent<PlayerData>();
            _playerDataLocal.SetNetPlayerData(playerData);
        }
    }
}