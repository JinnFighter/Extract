using UnityEngine;

namespace Ui
{
    public abstract class ScreenState : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
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