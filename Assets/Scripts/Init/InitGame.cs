using Common;
using Cysharp.Threading.Tasks;
using UiService;
using UnityEngine;
using VContainer;

namespace Init
{
    public class InitGame : MonoBehaviour
    {
        [Inject] private LoadingService _loadingService;
        [Inject] private NetworkService _networkService;
        [Inject] private IUiService _uiService;
        [Inject] private UserDataService _userDataService;

        private void Awake()
        {
            Debug.Log("Preparing to init");
            DontDestroyOnLoad(this);
        }

        private async void Start()
        {
            Debug.Log("Init started");
            await InitServices();
            Debug.Log("Init complete");

            _loadingService.LoadScene("MainMenu");
        }

        private async void OnApplicationQuit()
        {
            Debug.Log("Quit Requested, Terminating everything");
            await TerminateServices();
            Debug.Log("Terminate complete");
        }

        private async UniTask InitServices()
        {
            _loadingService.Init();
            _userDataService.Init();
            _networkService.Init();
            _uiService.Init();
        }

        private async UniTask TerminateServices()
        {
            _uiService.Terminate();
            _networkService.Terminate();
            _userDataService.Terminate();
            _loadingService.Terminate();
        }
    }
}