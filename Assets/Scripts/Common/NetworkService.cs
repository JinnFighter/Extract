using System;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace Common
{
    public class NetworkService : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerDataPrefab;
        [SerializeField] private NetworkManager _networkManager;

        public event Action<NetworkObject> OnClientConnected;

        public async UniTask<bool> Host()
        {
            if (_networkManager.ServerManager.Started) return false;

            try
            {
                _networkManager.ServerManager.StartConnection();
            }
            catch (Exception e)
            {
                return false;
            }

            await UniTask.WaitUntil(() => _networkManager.ServerManager.Started);
            _networkManager.ServerManager.OnRemoteConnectionState += HandleRemoteConnectionState;
            return true;
        }

        public void StopHost()
        {
            if (!_networkManager.ServerManager.Started) return;

            try
            {
                _networkManager.ServerManager.StopConnection(true);
            }
            catch (Exception e)
            {
            }
        }

        public async UniTask<bool> Connect(string address = "localhost")
        {
            if (_networkManager.ClientManager.Started) return false;
            _networkManager.ClientManager.OnClientConnectionState += HandleConnectionChanged;

            var finishedConnectionAttempt = false;
            var isSuccess = false;

            void HandleConnectionChanged(ClientConnectionStateArgs obj)
            {
                switch (obj.ConnectionState)
                {
                    case LocalConnectionState.Started:
                        isSuccess = true;
                        finishedConnectionAttempt = true;
                        break;
                    case LocalConnectionState.Stopped:
                        isSuccess = false;
                        finishedConnectionAttempt = true;
                        break;
                }
            }

            _networkManager.ClientManager.StartConnection(address);

            await UniTask.WaitUntil(() => finishedConnectionAttempt);
            _networkManager.ClientManager.OnClientConnectionState -= HandleConnectionChanged;
            if (isSuccess)
                await UniTask.WaitUntil(() =>
                    _networkManager.ClientManager.Started && _networkManager.ClientManager.Connection.IsValid);
            return isSuccess;
        }

        public bool IsHosting()
        {
            return _networkManager.ServerManager.Started;
        }

        public bool IsClientConnected()
        {
            return _networkManager.ClientManager.Started;
        }

        private void HandleRemoteConnectionState(NetworkConnection arg1, RemoteConnectionStateArgs arg2)
        {
            if (arg2.ConnectionState != RemoteConnectionState.Started) return;

            var playerData = Instantiate(_playerDataPrefab);
            _networkManager.ServerManager.Spawn(playerData.gameObject);
        }
    }
}