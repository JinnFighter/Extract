using System;
using Client.Actions.ActionBuilders;
using Logic.ActionRequests;

namespace Client.Actions
{
    public interface IActionRequestBuilderSystem
    {
        event Action OnReset;
        void Reset();
        IActionRequestBuilder StartBuild(EActionRequestType eActionRequestType);
        ActionRequest Build();
    }
}
