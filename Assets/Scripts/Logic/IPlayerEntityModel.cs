using System;
using Logic.ActionRequests;

namespace Logic
{
    public interface IPlayerEntityModel
    {
        event Action<ActionRequestOption> OnOptionAdded;
        event Action<ActionRequestOption> OnOptionRemoved;
        void AddOption(ActionRequestOption option);

        void RemoveOption(ActionRequestOption option);
    }
}