using Client.Descriptions;
using Common;
using Cysharp.Threading.Tasks;
using Logic.Descriptions;
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
        [Inject] private UnitDescriptionLibrary _unitDescriptionLibrary;
        [Inject] private UnitViewLibrary _unitViewLibrary;

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
            _unitDescriptionLibrary.Init();
            _unitViewLibrary.Init();
        }

        private async UniTask TerminateServices()
        {
            _unitDescriptionLibrary.Terminate();
            _unitViewLibrary.Terminate();
            _uiService.Terminate();
            _networkService.Terminate();
            _userDataService.Terminate();
            _loadingService.Terminate();
        }
    }
}