using UnityEngine.UI;

namespace UiService.Code.LayerHelpers
{
    public class DialogLayerHelper : ILayerHelper
    {
        private readonly Image _backgroundFader;

        public DialogLayerHelper(Image backgroundFader)
        {
            _backgroundFader = backgroundFader;
        }

        public void Init()
        {
            _backgroundFader.gameObject.SetActive(false);
        }

        public void Terminate()
        {
            _backgroundFader.gameObject.SetActive(false);
        }

        public void Open()
        {
            _backgroundFader.gameObject.SetActive(true);
        }

        public void Close()
        {
            _backgroundFader.gameObject.SetActive(false);
        }
    }
}