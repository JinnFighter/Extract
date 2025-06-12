using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Client
{
    public class ClickableGameObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent OnObjectClicked { get; } = new();
        public UnityEvent OnHoverEnter { get; } = new();
        public UnityEvent OnHoverExit { get; } = new();

        public bool IsHovered { get; private set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnObjectClicked.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
            OnHoverEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
            OnHoverExit.Invoke();
        }
    }
}