using System;
using Cysharp.Threading.Tasks;
using FishNet.Broadcast;
using FishNet.Component.Spawning;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;
using VContainer;
using Channel = FishNet.Transporting.Channel;

namespace Common
{
    public class NetworkService : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerDataPrefab;
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private PlayerSpawner _playerSpawner;

        [Inject] private LobbyService _lobbyService;

        public void Init()
        {
            _playerSpawner.OnSpawned += HandlePlayerObjectSpawned;
        }

        public void Terminate()
        {
            _playerSpawner.OnSpawned -= HandlePlayerObjectSpawned;
        }

        private void HandlePlayerObjectSpawned(NetworkObject obj)
        {
            _lobbyService.Players.Add(obj.GetComponent<PlayerData>());
        }

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

            _networkManager.ClientManager.StartConnection(address == "" ? "localhost" : address);

            await UniTask.WaitUntil(() => finishedConnectionAttempt);
            _networkManager.ClientManager.OnClientConnectionState -= HandleConnectionChanged;
            if (isSuccess)
                await UniTask.WaitUntil(() =>
                    _networkManager.ClientManager.Started && _networkManager.ClientManager.Connection.IsValid);
            return isSuccess;
        }

        public void SubscribeServerBroadcast<T>(Action<NetworkConnection, T, Channel> action)
            where T : struct, IBroadcast
        {
            SubscribeServerBroadcastInner(action);
        }

        public void UnsubscribeServerBroadcast<T>(Action<NetworkConnection, T, Channel> action)
            where T : struct, IBroadcast
        {
            UnsubscribeServerBroadcastInner(action);
        }

        public void SubscribeClientBroadcast<T>(Action<T, Channel> action) where T : struct, IBroadcast
        {
            SubscribeClientBroadcastInner(action);
        }

        public void UnsubscribeClientBroadcast<T>(Action<T, Channel> action) where T : struct, IBroadcast
        {
            UnsubscribeClientBroadcastInner(action);
        }

        public void SendServerBroadcast<T>(T broadcast) where T : struct, IBroadcast
        {
            SendServerBroadcastInner(broadcast);
        }

        public void SendClientBroadcast<T>(T broadcast) where T : struct, IBroadcast
        {
            SendClientBroadcastInner(broadcast);
        }

        private void SubscribeServerBroadcastInner<T>(Action<NetworkConnection, T, Channel> action)
            where T : struct, IBroadcast
        {
            _networkManager.ServerManager.RegisterBroadcast(action);
        }

        private void UnsubscribeServerBroadcastInner<T>(Action<NetworkConnection, T, Channel> action)
            where T : struct, IBroadcast
        {
            _networkManager.ServerManager.UnregisterBroadcast(action);
        }

        private void SubscribeClientBroadcastInner<T>(Action<T, Channel> action) where T : struct, IBroadcast
        {
            _networkManager.ClientManager.RegisterBroadcast(action);
        }

        private void UnsubscribeClientBroadcastInner<T>(Action<T, Channel> action) where T : struct, IBroadcast
        {
            _networkManager.ClientManager.UnregisterBroadcast(action);
        }

        private void SendServerBroadcastInner<T>(T broadcast) where T : struct, IBroadcast
        {
            _networkManager.ServerManager.Broadcast(broadcast);
        }

        private void SendClientBroadcastInner<T>(T broadcast) where T : struct, IBroadcast
        {
            _networkManager.ClientManager.Broadcast(broadcast);
        }
    }
}