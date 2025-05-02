using UnityEngine;

namespace Ui
{
    public abstract class ScreenBase : MonoBehaviour
    {
        private ScreenState _currentState;

        public void Init()
        {
            InitInner();
        }

        public void Terminate()
        {
            _currentState?.ExitState();
            TerminateInner();
        }

        protected virtual void InitInner()
        {
        }

        protected virtual void TerminateInner()
        {
        }

        public void SwitchState(ScreenState screenState)
        {
            _currentState?.ExitState();
            _currentState = screenState;
            _currentState?.EnterState();
        }
    }
}