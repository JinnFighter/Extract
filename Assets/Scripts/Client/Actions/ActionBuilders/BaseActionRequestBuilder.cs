using System.Collections.Generic;
using Logic.ActionRequests;

namespace Client.Actions.ActionBuilders
{
    public abstract class BaseActionRequestBuilder : IActionRequestBuilder
    {
        public int OwnerId { get; protected set; } = -1;
        public int CasterId { get; protected set; } = -1;
        public List<IActionRequestBuildStep> Steps { get; } = new();
        public abstract void Reset();
        public IActionRequestBuilder SetOwner(int ownerId)
        {
            OwnerId = ownerId;
            return this;
        }

        public IActionRequestBuilder SetCaster(int casterId)
        {
            CasterId = casterId;
            return this;
        }

        public bool Validate()
        {
            return ValidateInner();
        }
        
        public ActionRequest BuildAction()
        {
            return BuildActionInner();
        }

        protected abstract bool ValidateInner();

        protected abstract ActionRequest BuildActionInner();
    }
}