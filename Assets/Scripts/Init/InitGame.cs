using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Init
{
    public class InitGame : MonoBehaviour
    {
        [Inject] private LoadingService _loadingService;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            _loadingService.Init();
            _loadingService.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            _loadingService.Terminate();
        }
    }
}