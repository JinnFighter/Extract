using UnityEngine;
using UnityEngine.SceneManagement;

namespace Init
{
    public class InitGame : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}