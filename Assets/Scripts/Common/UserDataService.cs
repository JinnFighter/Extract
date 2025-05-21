using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public class UserDataService : MonoBehaviour
    {
        private readonly PlayerDataLocal _playerDataLocal = new();
        public IPlayerData LocalPlayer => _playerDataLocal;

        private void Awake()
        {
            var nickname = "Unknown";
            if (PlayerPrefs.HasKey("Nickname"))
            {
                nickname = PlayerPrefs.GetString("Nickname");
            }
            else
            {
                nickname = $"Player_{Random.Range(1000, 9999)}";
                PlayerPrefs.SetString("Nickname", nickname);
                PlayerPrefs.Save();
            }

            _playerDataLocal.SetNickname(nickname);
        }

        public void SetNetPlayerData(PlayerData playerData)
        {
            _playerDataLocal.SetNetPlayerData(playerData);
        }

        public void ResetNetPlayerData()
        {
            _playerDataLocal.ResetNetPlayerData();
        }
    }
}