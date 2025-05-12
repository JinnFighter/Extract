using UnityEngine;

namespace Ui
{
    public abstract class ScreenState : MonoBehaviour
    {
        protected ScreenBase Owner { get; private set; }
        public void SetOwner(ScreenBase screen)
        {
            Owner = screen;
        }

        public void EnterState()
        {
            EnterStatInner();
            gameObject.SetActive(true);
        }

        public void ExitState()
        {
            ExitStatInner();
            gameObject.SetActive(false);
        }

        protected virtual void EnterStatInner()
        {
        }

        protected virtual void ExitStatInner()
        {
        }
    }
}