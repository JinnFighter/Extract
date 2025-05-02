using System;
using Cysharp.Threading.Tasks;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

namespace Network
{
    public class NetworkService : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;

        private void Awake()
        {
            DontDestroyOnLoad(this);
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
                    default:
                        break;
                }
            }

            _networkManager.ClientManager.StartConnection(address);
            
            await UniTask.WaitUntil(() => finishedConnectionAttempt);
            _networkManager.ClientManager.OnClientConnectionState -= HandleConnectionChanged;
            if (isSuccess)
            {
                await UniTask.WaitUntil(() =>
                    _networkManager.ClientManager.Started && _networkManager.ClientManager.Connection.IsValid);
            }
            return isSuccess;
        }

        public bool IsHosting() => _networkManager.ServerManager.Started;
        public bool IsClientConnected() => _networkManager.ClientManager.Started;
    }
}