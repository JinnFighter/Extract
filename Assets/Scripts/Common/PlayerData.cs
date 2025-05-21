using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Common
{
    public interface IPlayerData
    {
        bool IsLocalPlayer { get; }
        string Nickname { get; }
        bool IsReady { get; }
        event Action<string> OnNicknameUpdated;
        event Action<bool> OnReadyUpdated;
        void SetReady(bool isReady);
        void SetNickname(string nickname);
    }

    public class PlayerDataLocal : IPlayerData
    {
        private PlayerData _netPlayerData;
        public bool IsLocalPlayer => true;
        public string Nickname => _netPlayerData?.Nickname;
        public bool IsReady => _netPlayerData?.IsReady ?? false;
        public event Action<string> OnNicknameUpdated;
        public event Action<bool> OnReadyUpdated;

        public void SetNetPlayerData(PlayerData netPlayerData)
        {
            _netPlayerData = netPlayerData;
        }

        public void ResetNetPlayerData()
        {
            _netPlayerData = null;
        }
        
        public void SetReady(bool isReady)
        {
            _netPlayerData?.SetReady(isReady);
        }

        public void SetNickname(string nickname)
        {
            _netPlayerData?.SetNickname(nickname);
        }
        
    }

    public class PlayerData : NetworkBehaviour, IPlayerData
    {
        bool IPlayerData.IsLocalPlayer => Owner.IsLocalClient;
        public string Nickname => _nickname.Value;
        public bool IsReady => _isReady.Value;

        private SyncVar<string> _nickname { get; } = new(new SyncTypeSettings
        {
            ReadPermission = ReadPermission.ExcludeOwner,
            WritePermission = WritePermission.ClientUnsynchronized
        });

        private SyncVar<bool> _isReady { get; } = new(new SyncTypeSettings
        {
            ReadPermission = ReadPermission.ExcludeOwner,
            WritePermission = WritePermission.ClientUnsynchronized
        });

        public event Action<string> OnNicknameUpdated;
        public event Action<bool> OnReadyUpdated;

        public override void OnStartServer()
        {
            AutoResolver.Resolve<LobbyService>().Players.Add(this);
        }

        public override void OnStopServer()
        {
            AutoResolver.Resolve<LobbyService>().Players.Add(this);
        }

        public override void OnStartClient()
        {
            if (IsOwner) SetNickname(PlayerPrefs.GetString("Nickname"));
            _nickname.OnChange += HandleNicknameChanged;
            _isReady.OnChange += HandleIsReadyChanged;
        }

        public override void OnStopClient()
        { 
            _nickname.OnChange -= HandleNicknameChanged;
            _isReady.OnChange -= HandleIsReadyChanged;
        }

        private void HandleIsReadyChanged(bool prev, bool next, bool asserver)
        {
            OnReadyUpdated?.Invoke(next);
        }

        private void HandleNicknameChanged(string prev, string next, bool asserver)
        {
            OnNicknameUpdated?.Invoke(next);
        }
        
        public void SetReady(bool isReady)
        {
            RpcSetReady(isReady);
        }

        public void SetNickname(string nickname)
        {
            RpcSetNickName(nickname);
        }

        [ServerRpc(RunLocally = true)]
        private void RpcSetNickName(string value)
        {
            _nickname.Value = value;
        }
        
        [ServerRpc(RunLocally = true)]
        private void RpcSetReady(bool isReady)
        {
            _isReady.Value = isReady;
        }
    }
}