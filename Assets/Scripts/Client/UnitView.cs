using UnityEngine;

namespace Client
{
    public class UnitView : MonoBehaviour
    {
        [field: SerializeField] public ClickableGameObject ClickableGameObject { get; private set; }
    }
}