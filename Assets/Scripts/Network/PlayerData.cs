using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Network
{
    public interface IPlayerData
    {
        SyncVar<string> Nickname { get; }
        SyncVar<bool> IsReady { get; }
        event Action<string> OnNicknameUpdated;
        event Action<bool> OnReadyUpdated;
    }

    public class PlayerData : NetworkBehaviour, IPlayerData
    {
        public SyncVar<string> Nickname { get; } = new(new SyncTypeSettings
        {
            ReadPermission = ReadPermission.ExcludeOwner,
            WritePermission = WritePermission.ClientUnsynchronized
        });

        public SyncVar<bool> IsReady { get; } = new(new SyncTypeSettings
        {
            ReadPermission = ReadPermission.ExcludeOwner,
            WritePermission = WritePermission.ClientUnsynchronized
        });

        public event Action<string> OnNicknameUpdated;
        public event Action<bool> OnReadyUpdated;

        public override void OnStartClient()
        {
            if (IsOwner) SetNickName(PlayerPrefs.GetString("Nickname"));
            Nickname.OnChange += HandleNicknameChanged;
            IsReady.OnChange += HandleIsReadyChanged;
        }

        public override void OnStopClient()
        {
            Nickname.OnChange -= HandleNicknameChanged;
            IsReady.OnChange -= HandleIsReadyChanged;
        }

        private void HandleIsReadyChanged(bool prev, bool next, bool asserver)
        {
            OnReadyUpdated?.Invoke(next);
        }

        private void HandleNicknameChanged(string prev, string next, bool asserver)
        {
            OnNicknameUpdated?.Invoke(next);
        }

        [ServerRpc]
        private void SetNickName(string value)
        {
            Nickname.Value = value;
        }

        [ServerRpc]
        public void SetReady(bool isReady)
        {
            IsReady.Value = isReady;
        }
    }
}