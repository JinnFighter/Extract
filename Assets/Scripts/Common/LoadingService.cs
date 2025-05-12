using FishNet.Managing;
using FishNet.Managing.Scened;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Common
{
    public class LoadingService : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;
        private readonly string _loadingSceneName = "Loading";
        private string _currentSceneName;
        private SceneLookupData _sceneLookupData;

        public void Init()
        {
            _networkManager.SceneManager.OnQueueStart += HandleQueueStart;
            _networkManager.SceneManager.OnQueueEnd += HandleQueueEnd;
        }

        public void Terminate()
        {
            _networkManager.SceneManager.OnQueueStart -= HandleQueueStart;
            _networkManager.SceneManager.OnQueueEnd -= HandleQueueEnd;
        }

        private async void HandleQueueStart()
        {
            await SceneManager.LoadSceneAsync(_loadingSceneName, LoadSceneMode.Additive);
            if (!string.IsNullOrEmpty(_currentSceneName))
            {
                await SceneManager.UnloadSceneAsync(_currentSceneName);
                _currentSceneName = "";
            }
        }

        private async void HandleQueueEnd()
        {
            await SceneManager.UnloadSceneAsync(_loadingSceneName);
        }

        public async void LoadScene(string sceneName, bool isNetworked = false)
        {
            if (isNetworked)
            {
                _sceneLookupData = new SceneLookupData(sceneName);
                var sld = new SceneLoadData(_sceneLookupData)
                {
                    Options = new LoadOptions
                    {
                        AutomaticallyUnload = false
                    },

                    ReplaceScenes = ReplaceOption.None,
                    PreferredActiveScene = new PreferredScene(_sceneLookupData)
                };

                _networkManager.SceneManager.LoadGlobalScenes(sld);
            }
            else
            {
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                _currentSceneName = sceneName;
            }
        }
    }
}