using System.Collections.Generic;
using Logic.ActionEvents;

namespace Client.Replay
{
    public abstract class BaseActionViewer<T> : IActionViewer where T : ActionEvent
    {
        public void Prepare(BattleInstanceClient battleInstance)
        {
            PrepareInner(battleInstance);
        }

        public IEnumerator<bool> Play(BattleInstanceClient battleInstance, ActionEvent actionEvent)
        {
            return PlayInner(battleInstance, (T)actionEvent);
        }

        public void Finish(BattleInstanceClient battleInstance)
        {
            FinishInner(battleInstance);
        }

        protected abstract IEnumerator<bool> PlayInner(BattleInstanceClient battleInstance, T actionEvent);

        protected virtual void PrepareInner(BattleInstanceClient battleInstance)
        {
        }
        protected virtual void FinishInner(BattleInstanceClient battleInstance)
        {
        }
    }
}