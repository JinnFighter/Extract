using System;
using System.Collections.Generic;
using Logic.ActionEvents;
using Logic.ActionRequests;

namespace Logic.Systems
{
    public interface ILogicSystem
    {
        public bool IsCancelled { get; }
        public event Action<ILogicSystem> OnPrepareLogicRunLaunched;
        public event Action<ILogicSystem> OnCancelLogicRunLaunched;
        public event Action<ILogicSystem> OnFinishLogicRunLaunched;
        IEnumerator<List<ActionEvent>> RunPrepareLogic(ActionRequest rootRequest, LogicModelServer modelServer);
        IEnumerator<List<ActionEvent>> RunLogic(ActionRequest rootRequest, LogicModelServer modelServer);
        IEnumerator<List<ActionEvent>> RunFinishLogic(ActionRequest rootRequest, LogicModelServer modelServer);
        IEnumerator<List<ActionEvent>> RunCancelLogic(ActionRequest rootRequest, LogicModelServer modelServer);
    }
}