using System;
using System.Collections.Generic;
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
        int Id { get; }
        List<string> Coterie { get; }
        event Action<string> OnNicknameUpdated;
        event Action<bool> OnReadyUpdated;
        void SetReady(bool isReady);
        void SetNickname(string nickname);
        void SetCoterieId(int id, string nameId);
        void AddCoterie(string nameId);
    }

    public class PlayerDataLocal : IPlayerData
    {
        private PlayerData _netPlayerData;
        private string _localNickname;
        public bool IsLocalPlayer => true;
        public string Nickname => _netPlayerData?.Nickname;
        public bool IsReady => _netPlayerData?.IsReady ?? false;
        public int Id => _netPlayerData?.OwnerId ?? 1;
        public List<string> Coterie => _netPlayerData == null ? _coterie : _netPlayerData.Coterie;
        public event Action<string> OnNicknameUpdated;
        public event Action<bool> OnReadyUpdated;

        private readonly List<string> _coterie = new();
        
        public void SetNetPlayerData(PlayerData netPlayerData)
        {
            if (_netPlayerData != null)
            {
                _netPlayerData.OnNicknameUpdated -= HandleNetNicknameUpdated;
            }
            _netPlayerData = netPlayerData;
            if (_netPlayerData != null)
            {
                _netPlayerData.OnNicknameUpdated += HandleNetNicknameUpdated;
                SetNickname(_localNickname);
                foreach (var coterie in _coterie)
                {
                    _netPlayerData.AddCoterie(coterie);
                }
            }
        }

        public void ResetNetPlayerData()
        {
            if (_netPlayerData != null)
            {
                _netPlayerData.OnNicknameUpdated -= HandleNetNicknameUpdated;
            }
            _netPlayerData = null;
        }
        
        public void SetReady(bool isReady)
        {
            _netPlayerData?.SetReady(isReady);
        }

        public void SetNickname(string nickname)
        {
            _localNickname = nickname;
            _netPlayerData?.SetNickname(nickname);
            PlayerPrefs.SetString("Nickname", _localNickname);
            PlayerPrefs.Save();
        }

        public void SetCoterieId(int id, string nameId)
        {
            if (_netPlayerData != null)
            {
                _netPlayerData.SetCoterieId(id, nameId);
            }
            else
            {
                _coterie[id] = nameId;
            }
            PlayerPrefs.SetString($"coterie_{id}", nameId);
            PlayerPrefs.Save();
        }

        public void AddCoterie(string nameId)
        {
            if (_netPlayerData != null)
            {
               _netPlayerData.AddCoterie(nameId);
            }
            else
            {
                _coterie.Add(nameId);
            }
            PlayerPrefs.SetString($"coterie_{Coterie.Count - 1}", nameId);
            PlayerPrefs.Save();
        }

        private void HandleNetNicknameUpdated(string name)
        {
            OnNicknameUpdated?.Invoke(name);
        }
    }

    public class PlayerData : NetworkBehaviour, IPlayerData
    {
        bool IPlayerData.IsLocalPlayer => Owner.IsLocalClient;
        public string Nickname => _nickname.Value;
        public bool IsReady => _isReady.Value;
        public int Id => OwnerId;
        public List<string> Coterie => _coterie.Collection;

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

        private SyncList<string> _coterie { get; } = new();

        public event Action<string> OnNicknameUpdated;
        public event Action<bool> OnReadyUpdated;

        public override void OnStartClient()
        {
            if (IsOwner)
            {
                AutoResolver.Resolve<UserDataService>().SetNetPlayerData(this);
                SetNickname(PlayerPrefs.GetString("Nickname"));
            }
            _nickname.OnChange += HandleNicknameChanged;
            _isReady.OnChange += HandleIsReadyChanged;
        }

        public override void OnStopClient()
        { 
            if (IsOwner)
            {
                AutoResolver.Resolve<UserDataService>().ResetNetPlayerData();
            }
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

        public void SetCoterieId(int id, string nameId)
        {
            RpcSetCoterieId(id, nameId);
        }

        public void AddCoterie(string nameId)
        {
            RpcAddCoterie(nameId);
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

        [ServerRpc(RunLocally = false)]
        private void RpcSetCoterieId(int id, string nameId)
        {
            _coterie[id] = nameId;
        }

        [ServerRpc(RunLocally = false)]
        private void RpcAddCoterie(string nameId)
        {
            _coterie.Add(nameId);
        }
    }
}