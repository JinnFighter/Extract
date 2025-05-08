using System;
using Cysharp.Threading.Tasks;
using FishNet.Managing;
using FishNet.Managing.Scened;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = FishNet.Managing.Scened.SceneManager;

namespace Common
{
    public class LoadingService : MonoBehaviour
    {
        [SerializeField] private NetworkManager _networkManager;
        private readonly string _loadingSceneName = "Loading";
        private string _currentSceneName;
        private bool _isLoading;
        private bool _isUnloading;
        private bool _isNetworked;
        private SceneLookupData _sceneLookupData;

        public void Init()
        {
        }

        public void Terminate()
        {
            _networkManager.SceneManager.OnLoadStart -= HandleLoadStart;
            _networkManager.SceneManager.OnLoadEnd -= HandleLoadEnd;
            _networkManager.SceneManager.OnUnloadStart -= HandleUnloadStart;
            _networkManager.SceneManager.OnUnloadEnd -= HandleUnloadEnd;
        }

        public async void LoadScene(string sceneName, bool isNetworked = false)
        {
            if (_isLoading || _isUnloading) return;

            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_loadingSceneName, LoadSceneMode.Additive)
                .ToUniTask();
            if (isNetworked)
            {
                var sceneLookupData = _sceneLookupData;
                if (!string.IsNullOrEmpty(_currentSceneName))
                {
                    if (_isNetworked)
                    {
                        UnloadNetworkedInner(sceneLookupData);
                    }
                    else
                    {
                        UnloadLocalInner(_currentSceneName);
                    }
                    await UniTask.WaitUntil(() => !_isUnloading);
                }
                LoadNetworkedInner(sceneName);
            }
            else
            {
                var currentScene = _currentSceneName;
                if (!string.IsNullOrEmpty(currentScene))
                {
                    if (_isNetworked)
                    {
                        UnloadNetworkedInner(_sceneLookupData);
                    }
                    else
                    {
                        UnloadLocalInner(_currentSceneName);
                    }
                    await UniTask.WaitUntil(() => !_isUnloading);
                }

                LoadLocalInner(sceneName);
            }

            await UniTask.WaitUntil(() => !_isLoading);
            await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_loadingSceneName).ToUniTask();
        }

        private void LoadNetworkedInner(string sceneName)
        {
            _isLoading = true;
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

            _networkManager.SceneManager.OnLoadStart += HandleLoadStart;
            _networkManager.SceneManager.OnUnloadEnd += HandleUnloadEnd;
            _networkManager.SceneManager.LoadGlobalScenes(sld);
        }

        private async void LoadLocalInner(string sceneName)
        {
            _isLoading = true;
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)
                .ToUniTask();
            _currentSceneName = sceneName;
            _isNetworked = false;
            _isLoading = false;
        }

        private async void UnloadLocalInner(string sceneName)
        {
            _isUnloading = true;
            await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName).ToUniTask();
            _isUnloading = false;
        }

        private void UnloadNetworkedInner(SceneLookupData sceneLookupData)
        {
            _isUnloading = true;
            var unloadData = new SceneUnloadData(sceneLookupData);

            _networkManager.SceneManager.OnUnloadStart += HandleUnloadStart;
            _networkManager.SceneManager.OnUnloadEnd += HandleUnloadEnd;
            _networkManager.SceneManager.UnloadGlobalScenes(unloadData);
        }

        private void HandleUnloadStart(SceneUnloadStartEventArgs obj)
        {
            _networkManager.SceneManager.OnUnloadStart -= HandleUnloadStart;
        }

        private void HandleUnloadEnd(SceneUnloadEndEventArgs obj)
        {
            _networkManager.SceneManager.OnUnloadEnd -= HandleUnloadEnd;
            _isUnloading = false;
        }

        private void HandleLoadEnd(SceneLoadEndEventArgs obj)
        {
            _networkManager.SceneManager.OnLoadEnd -= HandleLoadEnd;
            _currentSceneName = obj.LoadedScenes[0].name;
            _isNetworked = true;
            _isLoading = false;
        }

        private void HandleLoadStart(SceneLoadStartEventArgs obj)
        {
            _networkManager.SceneManager.OnLoadStart -= HandleLoadStart;
            _networkManager.SceneManager.OnLoadEnd += HandleLoadEnd;
        }
    }
}