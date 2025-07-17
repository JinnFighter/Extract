using UnityEngine;
using UnityEngine.UI;

namespace Ui.DebugWindows
{
    public class BattleDebugWindow : MonoBehaviour
    {
        [SerializeField] private Button _buttonDebug;
        [SerializeField] private Image _imageDebug;
        
        private void Awake()
        {
            _buttonDebug.onClick.AddListener(HandleButtonDebugClicked);
        }

        private void OnDestroy()
        {
            _buttonDebug.onClick.RemoveListener(HandleButtonDebugClicked);
        }

        private void HandleButtonDebugClicked()
        {
            _imageDebug.gameObject.SetActive(!_imageDebug.gameObject.activeSelf);
        }
    }
}
