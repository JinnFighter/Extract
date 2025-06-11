using System.Collections.Generic;
using Logic.ActionRequests;
using Logic.GameStateEvents;

namespace Logic.Systems
{
    public interface ILogicSystem
    {
        IEnumerator<List<GameStateEvent>> RunLogic(ActionRequest rootRequest, LogicModel model);
    }
}